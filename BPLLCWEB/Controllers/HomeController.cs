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
            Logins login = new Logins();
            login.UID = (int)TempData["UID"];
            login.UserName = (string)TempData["UserName"];
            login.SecretQuestion = (string)TempData["Question"];
            login.Errors = (int)TempData["Errors"];

            return View(login);
        }

        [HttpPost]
        public ActionResult ForgottenPassword(ValidateUser user)
        {
            bool valid = false;
            int errors = 0, uid = 0;
            string username = null, question = null;

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
                        if (Linqs.CheckUserName(ref uid, user.UserName, ref question, ref errors))
                        {
                            TempData["UID"] = uid;
                            TempData["UserName"] = user.UserName;
                            TempData["Question"] = question;
                            TempData["Errors"] = errors;
                            valid = true;
                        }
                    }
                    else
                    {
                        if (Linqs.CheckEmailAddress(user.EmailAdd, ref uid, ref username, ref question, ref errors))
                        {
                            TempData["UID"] = uid;
                            TempData["UserName"] = username;
                            TempData["Question"] = question;
                            TempData["Errors"] = errors;
                            valid = true;
                        }
                    }

                    if (!valid)
                    {
                        ViewBag.Message = "Incorrect entry. Please try again.";
                    }
                    else
                    {
                        if (errors > 2)
                        {
                            ViewBag.Message = "Your account has been locked. You can unlock your account by contacting the web administrator via email at sung.park@businesspartnersllc.com or john.sowter@businesspartnersllc.com.";
                        }
                        else
                        {
                            return RedirectToAction("SecretQA");
                        }
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
        public ActionResult SecretQA(Logins logins)
        {
            bool valid = false;
            var uid = logins.UID;
            var username = logins.UserName;
            int errors = logins.Errors;

            TempData["UID"] = logins.UID;
            TempData["UserName"] = logins.UserName;
            TempData["Question"] = logins.SecretQuestion;
            TempData["Errors"] = logins.Errors;

            try
            {
                if (String.IsNullOrEmpty(logins.SecretQuestion) || String.IsNullOrEmpty(logins.SecretAnswer))
                {
                    TempData["Message"] = "Secret Question and Secret Answer need to be entered.";
                    return RedirectToAction("SecretQA");
                }
                else
                {
                    // validate question and answer - username will always be present
                    if (!String.IsNullOrEmpty(logins.SecretQuestion) && !String.IsNullOrEmpty(logins.SecretAnswer))
                    {
                        if (username != null)
                        {
                            if (Linqs.CheckAnswer(username, logins.SecretQuestion, logins.SecretAnswer))
                            {
                                string emailAdd = null;

                                if (Linqs.GetEmailAddress(ref emailAdd, uid))
                                {
                                    if (Linqs.UpdateLoginRecord(processor, emailAdd, Convert.ToString(username), uid))
                                    {
                                        valid = true;
                                        TempData["Message"] = "Your will receive a new password soon.";
                                        return RedirectToAction("SecretQA");
                                    }
                                }
                            }
                            else
                            {
                                errors += 1;
                                
                                if(errors == 3)
                                {
                                    TempData["Errors"] = errors;
                                    Linqs.IncrementErrors(uid);

                                    // display account locked out message
                                    TempData["Message"] = "Your account has been locked. You can unlock your account by contacting the web administrator via email at sung.park@businesspartnersllc.com or john.sowter@businesspartnersllc.com.";
                                    return RedirectToAction("SecretQA");
                                }
                                else if(errors < 3)
                                {
                                    TempData["Errors"] = errors;
                                    Linqs.IncrementErrors(uid);
                                }
                            }
                        }
                    }

                    // display error 
                    if (!valid)
                    {
                        if(errors > 3)
                        {
                            TempData["Message"] = "Your account has been locked. You can unlock your account by contacting the web administrator via email at sung.park@businesspartnersllc.com or john.sowter@businesspartnersllc.com.";
                        }
                        else
                        {
                            TempData["Message"] = "Incorrect entry. You have " + (3 - errors) + " tries left.";
                        }                        
                        return RedirectToAction("SecretQA");
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
                    //mailtoAddress = "sung.park@businesspartnersllc.com";

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
                    //mailtoAddress = "sung.park@businesspartnersllc.com";

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