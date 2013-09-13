using System;
using System.Web;
namespace Blog.Repository
{
    public class NHibernateSessionManager : Interface.INHibernateSessionManager
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        private static readonly string currentKey = "NHibernateSession";

        /// <summary>
        /// 取得Session
        /// </summary>
        /// <returns></returns>
        public NHibernate.ISession GetInstance()
        {
            
            NHibernate.ISession session = null;

            #region DEBUG Code
            if (HttpContext.Current == null)
            {
                System.Web.HttpContext.Current = new System.Web.HttpContext(new System.Web.HttpRequest("", "http://localhost/", ""), new System.Web.HttpResponse(new System.IO.StringWriter()));
            }

            if (HttpContext.Current.Items.Contains(currentKey))
            {
                session = HttpContext.Current.Items[currentKey] as NHibernate.ISession;
            }
            else
            {
                session = new NHibernate.Cfg.Configuration().Configure("Config/hibernate.cfg.xml").BuildSessionFactory().OpenSession();
                HttpContext.Current.Items.Add(currentKey, session);
            }
            #endregion
            return session;
        }

        /// <summary>
        /// 释放Session
        /// </summary>
        public void Dispose()
        {
            GetInstance().Dispose();
        }
    }
}
