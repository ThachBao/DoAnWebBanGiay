using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebBanGiay.Filters
{
    public class AuthFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var authCookie = HttpContext.Current.Request.Cookies["auth"];
            if (authCookie == null || authCookie.Value == "")
            {
                filterContext.Result = new RedirectResult("/Users/Login");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}