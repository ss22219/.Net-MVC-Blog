using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Blog.WebUI.Models
{
    /// <summary>
    /// Session管理类，实现Domain中的SessionServices
    /// </summary>
    public class SessionManager : Blog.Domain.Interface.ISessionManager
    {
        Model.User Domain.Interface.ISessionManager.User
        {
            get
            {
                if (HttpContext.Current.Session["User"] == null)
                {
                    return null;
                }
                else
                {
                    return (Blog.Model.User)HttpContext.Current.Session["User"];
                }
            }
            set
            {
                HttpContext.Current.Session["User"] = value;
            }
        }

        public object Get(string name)
        {
            return HttpContext.Current.Session[name];
        }

        public void Set(string name, object value)
        {
            HttpContext.Current.Session[name] = value;
        }
    }
}