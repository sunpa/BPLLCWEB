using BPLLCWEB.Domain.Entities;
using BPLLCWEB.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BPLLCWEB.Classes;
using BPLLCWEB.Helpers;

namespace BPLLCWEB.Controllers
{
    public class ContactUsController : Controller
    {
        private IRepository repository;
        private IProcessor processor;

        public ContactUsController(IRepository repo, IProcessor proc)
        {
            repository = repo;
            processor = proc;
        }

        public ActionResult Contacts()
        {
            return View();
        }

        public ActionResult ContactUs(string contactcode)
        {
            if (String.IsNullOrEmpty(contactcode))
            {
                return RedirectToAction("/Contacts");
            }
            else
            {
                ViewBag.ContactCode = HttpUtility.HtmlEncode(contactcode);
                ViewBag.Subject = Reusables.ContactSubject(ViewBag.ContactCode);

                return View(new ContactInfo());
            }
        }

        [HttpPost]
        public ActionResult ContactUs(ContactInfo contactinfo)
        {
            ViewBag.Subject = Request["subject"]; // passing the value again to ViewBag.Message

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

                    string mailtoAddress = Reusables.ContactRecipient(contactinfo.ContactCode);

                    //*** for testing ***
                    //mailtoAddress = "sung.park@businesspartnersllc.com";

                    contactinfo.MailToAddress = mailtoAddress;
                    string subject = ViewBag.Subject;

                    if (!String.IsNullOrEmpty(mailtoAddress))
                    {
                        // send email
                        processor.ProcessContactInfo(contactinfo);
                    }
                }
            }
            return View();
        }
	}
}