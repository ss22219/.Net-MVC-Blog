using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blog.WebUI
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                null, // 路由名称
                "Article/{id}/{page}", // 带有参数的 URL
                new { controller = "Article", action = "Index", page = UrlParameter.Optional }
                // 参数默认值
                , new { id = "\\d+", page = "\\d*" }//规则
                , new string[] { "Blog.WebUI.Controllers" }
            );

            routes.MapRoute(
                null, // 路由名称
                "Article/{id}-{commentId}", // 带有参数的 URL
                new { controller = "Article", action = "Index" }// 参数默认值
                , new { id = "\\d+", page = "\\d*", commentId = "\\d*" }//规则
                , new string[] { "Blog.WebUI.Controllers" }
            );

            routes.MapRoute(
                null, // 路由名称
                "Search/{id}/{page}", // 带有参数的 URL
                new { controller = "Search", action = "Index", page = UrlParameter.Optional }
                // 参数默认值
                , new { page = "\\d*" }//规则
                , new string[] { "Blog.WebUI.Controllers" }
            );

            routes.MapRoute(
                "Base", // 路由名称
                "{controller}/{action}/{id}/{page}", // 带有参数的 URL
                new { controller = "Home", action = "Index", page = 1 }, // 参数默认值
                new { page = "\\d+" }
                , new string[] { "Blog.WebUI.Controllers" }
            );

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
                , new string[] { "Blog.WebUI.Controllers" }
            );

        }

        public void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //加载依赖注入组件
            Bootstrapper.Initializ(Server.MapPath("~/"));

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}