using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using Blog.Domain.Interface;

namespace Blog.WebUI.Areas.Admin
{
    /// <summary>
    /// 管理员权限检查属性
    /// </summary>
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!DependencyResolver.Current.GetService<IRoleService>().AddArticlePower())
            {
                filterContext.Result = new RedirectResult("/Member/Login?ReturnUrl=" + filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.Url.AbsoluteUri));
            }
        }
    }
}