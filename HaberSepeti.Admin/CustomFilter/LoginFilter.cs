using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HaberSepeti.Admin.CustomFilter
{
    public class LoginFilter : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            HttpContextWrapper wrapper = new HttpContextWrapper(HttpContext.Current);

            var SessionControl = context.HttpContext.Session["UserId"];
            if(SessionControl == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}