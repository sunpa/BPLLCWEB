using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BPLLCWEB.Domain.Entities;
using BPLLCWEB.Domain.Abstract;
using BPLLCWEB.Domain.Concrete;
using BPLLCWEB.Classes;
using BPLLCWEB.Helpers;
using System.Configuration;
using System.Text;
using System.Data.Entity;

namespace BPLLCWEB.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repository;
        private IProcessor processor;

        public HomeController(IRepository repo, IProcessor proc)
        {
            repository = repo;
            processor = proc;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult WhoWeAre()
        {
            return View();
        }

        public ActionResult PartnerWithUs()
        {
            return View();
        }
        public ActionResult Careers()
        {
            return View();
        }

        public ActionResult ServicesOrigination()
        {
            return View();
        }

        public ActionResult ServicesProcessing()
        {
            return View();
        }

        public ActionResult ServicesAsset()
        {
            return View();
        }

        public ActionResult ServicesLoanReferral()
        {
            return View();
        }

        public ActionResult PortfolioManagement()
        {
            return View();
        }

        public ActionResult LoansGeneral()
        {
            return View();
        }

        public ActionResult LoansSBA()
        {
            return View();
        }

        public ActionResult LoansCRE()
        {
            return View();
        }

        public ActionResult LoansSBS()
        {
            return View();
        }

        public ActionResult ParticipationGeneral()
        {
            return View();
        }

        public ActionResult ParticipationSignUp()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult IndustryNews()
        {
            return View();
        }    

        public ActionResult REONotesSale()
        {
            return View();
        }

        public ActionResult RecentFundings()
        {
            return View();
        }

        public ActionResult Broker()
        {
            return View();
        }

        public ActionResult ForgottenPassword()
        {
            return View();
        }

        public ActionResult SecretQA()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgottenPassword(ValidateUser user)
        {
            bool valid = false;

            try
            {
                if (String.IsNullOrEmpty(user.UserName) && String.IsNullOrEmpty(user.EmailAdd))
                {
                    ViewBag.Message = "Either Username Or Email Address needs to be entered.";
                }
                else
                {
                    // check if username OR email address is valid
                    if (!String.IsNullOrEmpty(user.UserName))
                    {                     
                        using (EFDbContext context = new EFDbContext())
                        {
                            var uname = (from u in context.logins
                                         where u.UserName == user.UserName
                                         select u).FirstOrDefault();

                            if (uname != null)
                            {
                                Session["UID"] = uname.UID;
                                Session["UserName"] = uname.UserName;
                                valid = true;
                            }
                        }
                    }
                    else
                    {
                        using (EFDbContext context = new EFDbContext())
                        {
                            var eadd = (from e in context.users
                                        where e.EmailAdd == user.EmailAdd
                                        select e).FirstOrDefault();

                            if (eadd != null)
                            {
                                Session["EmailAdd"] = eadd.EmailAdd;

                                // get username
                                var username = (from u in context.logins
                                                where u.UID == eadd.uniqueID
                                                select u).FirstOrDefault();

                                if (username != null)
                                {
                                    Session["UserName"] = username.UserName;
                                }

                                valid = true;
                            }
                        }
                    }

                    if (!valid)
                    {
                        ViewBag.Message = "Incorrect entry. Please try again.";
                    }
                    else
                    {
                        return RedirectToAction("/SecretQA");
                    }
                }
            }
            catch (Exception ex)
            {
                processor.ProcessSendErrorEmail(ex.Message + "\n\n" + ex.ToString(), "BP ForgottenPassword");
                ViewBag.Message = "Some error has occurred. Please try again later.";                
            }

            return View();
        }

        [HttpPost]
        public ActionResult SecretQA(SecretQA qa)
        {
            bool valid = false;
            var UID = Session["UID"];
            var username = Session["UserName"];

            try
            {
                if (String.IsNullOrEmpty(qa.SecretQuestion) || String.IsNullOrEmpty(qa.SecretAnswer))
                {
                    ViewBag.Message = "Secret Question and Secret Answer need to be entered.";
                }
                else
                {
                    // validate question and answer
                    if (!String.IsNullOrEmpty(qa.SecretQuestion) && !String.IsNullOrEmpty(qa.SecretAnswer))
                    {
                        if (username != null)
                        {
                            using (EFDbContext context = new EFDbContext())
                            {
                                var login = (from l in context.logins
                                             where l.SecretQuestion == qa.SecretQuestion
                                             && l.SecretAnswer == qa.SecretAnswer
                                             && l.UserName == (string)username
                                             select l).FirstOrDefault();

                                if (login != null)
                                {
                                    valid = true;
                                }
                            }
                        }
                    }

                    // display error if user login info is not found
                    if (!valid)
                    {
                        ViewBag.Message = "Incorrect entry. Please try again.";
                    }
                    else
                    {
                        string emailAdd = null;

                        if (Session["EmailAdd"] != null)
                        {
                            emailAdd = Session["EmailAdd"].ToString();
                        }
                        else if (UID != null)
                        {
                            using (EFDbContext context = new EFDbContext())
                            {
                                var ea = (from e in context.users
                                          where e.uniqueID == (int)UID
                                          select e).FirstOrDefault();

                                if (ea != null)
                                {
                                    emailAdd = ea.EmailAdd;
                                }
                                else
                                {
                                    valid = false;
                                }
                            }
                        }
                        else
                        {
                            // both UID and EmailAdd sessions are gone
                            valid = false;
                        }

                        if (!valid)
                        {
                            ViewBag.Message = "Please try again.";
                            return RedirectToAction("/ForgottenPassword");
                        }
                        else
                        {
                            // insert sercrets

                            // generate new password
                            string newPasword = Reusables.GetPassword();

                            // update login record

                            // send welcome email
                            processor.ProcessNewPasswordSendEmail(emailAdd, Convert.ToString(username), newPasword);

                            ViewBag.Message = "Your will receive a new password soon.";
                            Session["UserName"] = null;
                            Session["UID"] = null;
                            Session["EmailAdd"] = null;

                            //return RedirectToAction("/ForgottenPassword");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                processor.ProcessSendErrorEmail(ex.Message + "\n\n" + ex.ToString(), "BP SecretQA");
                ViewBag.Message = "Some error has occurred. Please try again later.";
            }
            return View();
        }


        [HttpPost]
        public ActionResult Broker(BrokerInfo brokerinfo)
        {
            if (ModelState.IsValid)
            {
                if (!ArithmeticCaptcha.isValid(Request["ArithmeticValue"], Request["ArithmeticValueEnc"]))
                {
                    //ModelState.AddModelError("ArithmeticValue", "Wrong number. Please try again.");
                    ViewBag.WrongNumber = "Wrong number. Please try again.";
                }
                else
                {
                    ViewBag.Successful = "Your Form has been submitted successfully!";
                    ViewBag.WrongNumber = "";

                    string mailtoAddress = ConfigurationManager.AppSettings["Email.To.Support"];
                    
                    //*** for testing ***
                    mailtoAddress = "sung.park@businesspartnersllc.com";

                    if (!String.IsNullOrEmpty(mailtoAddress))
                    {
                        brokerinfo.Subject = "Broker Request for Information";
                        brokerinfo.MailToAddress = mailtoAddress;

                        // send email
                        processor.ProcessBrokerInfo(brokerinfo);
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult ParticipationSignUp(ParticipantInfo participantinfo)
        {
            if (ModelState.IsValid)
            {
                if (!ArithmeticCaptcha.isValid(Request["ArithmeticValue"], Request["ArithmeticValueEnc"]))
                {
                    //ModelState.AddModelError("ArithmeticValue", "Wrong number. Please try again.");
                    ViewBag.WrongNumber = "Wrong number. Please try again.";
                }
                else
                {
                    ViewBag.Successful = "Your Form has been submitted successfully!";
                    ViewBag.WrongNumber = "";

                    StringBuilder sb = new StringBuilder();
                    string mailtoAddress = ConfigurationManager.AppSettings["Email.To.General"];

                    // get interested in items
                    string cre = Request.Form.GetValues("CRE")[0];
                    string sbs = Request.Form.GetValues("SBS")[0];
                    string sba = Request.Form.GetValues("SBA")[0];
                    string mbl = Request.Form.GetValues("MBL")[0];

                    if (cre == "true")
                    {
                        sb.AppendLine("Commercial Real Estate");
                    }
                    if (sbs == "true")
                    {
                        sb.AppendLine("Small Business Services");
                    }
                    if (sba == "true")
                    {
                        sb.AppendLine("Small Business Administration");
                    }
                    if (mbl == "true")
                    {
                        sb.AppendLine("Member Business Lending");
                    }


                    //*** for testing ************************************
                    mailtoAddress = "sung.park@businesspartnersllc.com";

                    if (!String.IsNullOrEmpty(mailtoAddress))
                    {
                        participantinfo.InterestedIn = sb.ToString();
                        participantinfo.Subject = "Participant Request Form";
                        participantinfo.MailToAddress = mailtoAddress;

                        // send email
                        processor.ProcessParticipantInfo(participantinfo);
                    }
                }
            }
            return View();
        }


        public ActionResult DownloadApplicationFile()
        {
            var buffer = "/Downloads/Referral_Application.pdf";
            return File(buffer, "application/pdf");
        }

        public ActionResult DownloadRateSheetFile()
        {
            var buffer = "/Downloads/BPLLC-Rate-Sheet.pdf";
            return File(buffer, "application/pdf");
        }

    }
}