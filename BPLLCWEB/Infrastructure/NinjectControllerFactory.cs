using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using BPLLCWEB.Domain.Entities;
using BPLLCWEB.Domain.Abstract;
using BPLLCWEB.Domain.Concrete;
using System.Configuration;

namespace BPLLCWEB.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory() {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IRepository>().To<EFRepository>();



            EmailSettings emailSettings = new EmailSettings
            {
                // not needed for now
                //WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsfile"] ?? "false")
            };

            ninjectKernel.Bind<IProcessor>().To<SubmitProcessor>()
                .WithConstructorArgument("settings", emailSettings);
        }
    }
}