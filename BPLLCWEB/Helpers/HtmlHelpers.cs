using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BPLLCWEB.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            string currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            string currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            if ((actionName == currentAction || CheckCurrentAction(currentAction)) && controllerName == currentController)
            {
                return htmlHelper.ActionLink(
                    linkText,
                    actionName,
                    controllerName,
                    null,
                    new
                    {
                        @class = "current"
                    });
            }
            else
            {
                return htmlHelper.ActionLink(linkText, actionName, controllerName);
            }
        }


        private static bool CheckCurrentAction(string currentAction)
        {
            if (currentAction == "Edit" || currentAction == "Create" || currentAction == "Area")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}