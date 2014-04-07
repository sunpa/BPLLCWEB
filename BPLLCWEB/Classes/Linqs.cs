using BPLLCWEB.Domain.Abstract;
using BPLLCWEB.Domain.Concrete;
using BPLLCWEB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BPLLCWEB.Classes
{
    public class Linqs
    {
        private static int lockoutAttempts = 9;

        public static bool DetemineLock(string ipaddress)
        {
            bool valid = false;

            using (EFDbContext context = new EFDbContext())
            {
                DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                var loginattempts = (from la in context.loginAttempts
                                     where la.ClientIP == ipaddress
                                     && la.Unlocked == 0
                                     && la.DateAdded >= currentDate
                                     select la).AsEnumerable();

                int count = loginattempts.Count();
                if(count > lockoutAttempts)
                {
                    valid = true; // account locked
                }
            }
            return valid;
        }


        public static bool ValidateLogin(string username, string plainpassword)
        {
            bool valid = false;
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes = null;
            UTF8Encoding encoder = null;

            using (EFDbContext context = new EFDbContext())
            {
                encoder = new UTF8Encoding();
                hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(plainpassword));

                var login = (from lo in context.logins
                             where lo.UserName == username
                             && lo.EncryptedPassword == hashedDataBytes
                             select lo).FirstOrDefault();

                if (login != null)
                {
                    valid = true;
                }
            }
            return valid;
        }

        public static bool CheckUserName(ref int uid, string username, ref string question, ref int errors)
        {
            bool valid = false;

            using (EFDbContext context = new EFDbContext())
            {
                var uname = (from u in context.logins
                             where u.UserName == username
                             select u).FirstOrDefault();

                if (uname != null)
                {
                    uid = uname.UID;
                    username = uname.UserName;
                    question = uname.SecretQuestion;
                    errors = uname.Errors;
                    valid = true;
                }
                else
                {
                    valid = false;
                }
            }
            return valid;
        }

        public static bool CheckEmailAddress(string email, ref int uid, ref string username, ref string question, ref int errors)
        {
            bool valid = false;

            using (EFDbContext context = new EFDbContext())
            {
                var eadd = (from e in context.users
                            where e.EmailAdd == email
                            select e).FirstOrDefault();

                if (eadd != null)
                {
                    // get login info
                    var uname = (from u in context.logins
                                    where u.UID == eadd.uniqueID
                                    select u).FirstOrDefault();

                    if (uname != null)
                    {
                        uid = uname.UID;
                        username = uname.UserName;
                        question = uname.SecretQuestion;
                        errors = uname.Errors;
                        valid = true;
                    }
                    else
                    {
                        valid = false;
                    }
                }
                else
                {
                    valid = false;
                }
            }
            return valid;
        }

        public static bool CheckAnswer(string username, string question, string answer)
        {
            bool valid = false;

            using (EFDbContext context = new EFDbContext())
            {
                var login = (from l in context.logins
                             where l.SecretQuestion == (string)question
                             && l.SecretAnswer == (string)answer
                             && l.UserName == (string)username
                             select l).FirstOrDefault();

                if (login != null)
                {
                    valid = true;
                }
                else
                {
                    valid = false;
                }
            }
            return valid;
        }

        public static bool GetEmailAddress(ref string email, int uid)
        {
            bool valid = false;

            using (EFDbContext context = new EFDbContext())
            {
                var ea = (from e in context.users
                          where e.uniqueID == (int)uid
                          select e).FirstOrDefault();

                if (ea != null)
                {
                    valid = true;
                    email = ea.EmailAdd;
                }
                else
                {
                    valid = false;
                }
            }
            return valid;
        }

        public static void InsertLoginAttempt(string ipaddress)
        {
            using (EFDbContext context = new EFDbContext())
            {
                LoginAttempts la = new LoginAttempts();
                
                la.ClientIP = ipaddress;
                la.Unlocked = 0;
                la.DateAdded = DateTime.Now;

                context.loginAttempts.Add(la);
                context.SaveChanges();
            }
        }

        public static bool UpdateLoginRecord(IProcessor processor, string email, string username, int uid)
        {
            bool valid = false;

            try
            {
                // generate new password
                string newPasword = Reusables.GetPassword();

                // update login record
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] hashedDataBytes = null;
                UTF8Encoding encoder = new UTF8Encoding();

                hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(newPasword));

                using (EFDbContext context = new EFDbContext())
                {
                    var login = (from l in context.logins
                                 where l.UID == uid
                                 select l).FirstOrDefault();

                    if (login != null)
                    {
                        login.Errors = 0;
                        login.Modified_On = DateTime.Now;
                        login.EncryptedPassword = hashedDataBytes;

                        // *** marked out for testing
                        context.SaveChanges();
                    }
                }

                // send welcome email
                processor.ProcessNewPasswordSendEmail(email, username, newPasword);

                valid = true;
            }
            catch (Exception ex)
            {

            }
            return valid;
        }


        public static void IncrementErrors(int uid)
        {
            using (EFDbContext context = new EFDbContext())
            {
                var login = (from l in context.logins
                             where l.UID == uid
                             select l).FirstOrDefault();

                if (login != null)
                {
                    login.Errors = login.Errors + 1;
                    login.Modified_On = DateTime.Now;

                    context.SaveChanges();
                }
            }
        }


    }
}