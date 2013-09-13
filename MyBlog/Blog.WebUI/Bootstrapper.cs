using System.Reflection;
using System.Web.Mvc;
using Autofac;
using System.Collections.Generic;
using Autofac.Integration.Mvc;
using NHibernate;
using NHibernate.Cfg;
using Blog.WebUI.Models;
using Blog.Domain.Interface;
using System.Web.Caching;
using System.Web;

namespace Blog.WebUI
{

    /// <summary>
    /// 应用程序入口
    /// 
    /// Author: Gool
    /// Update: 2013-8-6 14:14
    /// </summary>
    public class Bootstrapper
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="path">程序运行物理路径</param>
        public static void Initializ(string path)
        {
            ContainerBuilder builder = new ContainerBuilder();

            //注册容器的作用域，本程序集下所有控制器 在Controller激活后，将Setting配置加载到ViewData中
            builder.RegisterControllers(Assembly.GetExecutingAssembly())
                .Where(
                    controller => typeof(IController).IsAssignableFrom(controller) && controller.Name.EndsWith("Controller"))
                .OnActivated(
                    handler => LoadSetting(handler)
            );

            //注册SessionFactory组件到容器,并将其生命周期设置为单例实例
            builder.Register<ISessionFactory>(c => new Configuration().Configure(path + "bin/hibernate.cfg.xml").BuildSessionFactory()).SingleInstance();

            //注册Session组件到容器，生命周期为HTTP上下文
            builder.Register<ISession>(
                c => c.Resolve<ISessionFactory>().OpenSession()).OnRelease(s => s.Flush()).InstancePerHttpRequest();

            //注册缓存依赖到容器，生命周期为Application
            builder.Register<CacheDependency>(c => new SqlCacheDependency("Default", "Setting"));
            builder.Register<CacheDependency>(c => new SqlCacheDependency("Default", "Article")).Named<CacheDependency>("Article").SingleInstance();
            builder.Register<CacheDependency>(c => new SqlCacheDependency("Default", "Comment")).Named<CacheDependency>("Comment").SingleInstance();

            ///注册各业务实现类
            builder.Register<ISessionManager>(c => new SessionManager()).SingleInstance();


            //从Repository与Domain程序自动加载组件
            builder.RegisterAssemblyTypes(new Assembly[] { Assembly.Load("Blog.Repository"), Assembly.Load("Blog.Domain") })
                .Where(
                    t => t.Name.EndsWith("Service") || t.Name.EndsWith("Repository")
            ).InstancePerHttpRequest().AsImplementedInterfaces();

            //构造容器对象
            IContainer container = builder.Build();

            //将容器设置到MVC依赖注入提供者中
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        //加载配置到ViewBag
        public static void LoadSetting(Autofac.Core.IActivatedEventArgs<object> handler)
        {
            ControllerBase controllerBase = handler.Instance as ControllerBase;
            IDictionary<string, string> settings = handler.Context.Resolve<ISettingService>().GetAllSetting();
            foreach (var setting in settings)
            {
                controllerBase.ViewData[setting.Key] = setting.Value;
            }
            controllerBase.ViewData["User"] = handler.Context.Resolve<IUserService>().GetLoginUser();
        }
    }
}