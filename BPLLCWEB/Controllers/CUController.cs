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
    public class LogSessionController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (System.Web.HttpContext.Current.Session["LoggedUser"] == null)
            {
                filterContext
                    .HttpContext
                    .Response
                    .Redirect("/Home/Login/1");  // session expired              
            }           
        }
    }

    public class CUController : LogSessionController
    {
        private IRepository repository;
        private IProcessor process;

        public CUController(IRepository repo, IProcessor proc)
        {
            repository = repo;
            process = proc;
        }

 
        // GET: /CU/
        public ActionResult Index()
        {

            var login = System.Web.HttpContext.Current.Session["LoggedUser"];

            //if (login != null)
            //{
            //    return View();
            //}
            //else
            //{
            //    TempData["Message"] = "Your session has expired. Please login again.";
            //    return RedirectToAction("Login", "Home");
            //}

            return View();
        }
	}
}