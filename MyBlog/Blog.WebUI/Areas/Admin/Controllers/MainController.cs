using Blog.Domain;
using Blog.Domain.Interface;
using Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.WebUI.Areas.Admin.Controllers
{
    /// <summary>
    /// 后台入口
    /// </summary>
    public class MainController : Controller
    {
        private ISessionManager _sessionManager;
        public MainController(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        [AdminAuthorize]
        public ActionResult Index()
        {
            return RedirectToAction("About");
        }

        [AdminAuthorize]
        public ActionResult About()
        {
            return View();
        }
    }
}
