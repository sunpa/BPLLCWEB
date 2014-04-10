using BPLLCWEB.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using BPLLCWEB.Domain.Concrete;
using System.Configuration;

namespace BPLLCWEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());

            Database.SetInitializer<EFDbContext>(null);
        }

        protected void Application_BeginRequest()
        {
            if (!Context.Request.IsSecureConnection)
                if (ConfigurationManager.AppSettings["SecureSite"] == "1")
                {
                    Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"));
                }
        }
    }
}
