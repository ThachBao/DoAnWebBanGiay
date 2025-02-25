using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebBanGiay.Filters
{
    public class AdminFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var roleCookie = HttpContext.Current.Request.Cookies["role"];
            if (roleCookie == null || roleCookie.Value != "admin")
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { controller = "Users", action = "Login", area = "" }
                    )
                );
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
