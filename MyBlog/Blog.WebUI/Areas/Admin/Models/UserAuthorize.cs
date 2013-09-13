using Blog.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.WebUI.Areas.Admin
{
    public class UserAuthorizeAttribute : AdminAuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!DependencyResolver.Current.GetService<IRoleService>().LoginPower())
            {
                filterContext.Result = new RedirectResult("/Member/Login?ReturnUrl=" + filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.Url.AbsoluteUri));
            }
        }
    }
}