using Blog.Domain.Interface;
using Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.WebUI.Areas.Admin.Controllers
{
    public class HeadController : Controller
    {
        private IUserService _userService;
        private IRoleService _roleService;
        private ICommentService _commentService;

        public HeadController(IUserService userService, IRoleService roleService, ICommentService commentService)
        {
            _userService = userService;
            _roleService = roleService;
            _commentService = commentService;
        }
        /// <summary>
        /// 控件
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            User user = _userService.GetLoginUser();
            ViewBag.User = user;
            ViewBag.VerifyCommentCount = _commentService.GetVerifyCommentCount();
            return View("_Head", ViewBag.VerifyCommentCount);
        }

    }
}
