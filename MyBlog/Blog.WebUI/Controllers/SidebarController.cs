using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.WebUI.Models;
using Blog.Domain.Interface;
namespace Blog.WebUI.Controllers
{
    /// <summary>
    /// 通用，工具菜单，这个是控件
    /// </summary>
    public class SidebarController : Controller
    {
        /// <summary>
        /// 这个，还是容器注入
        /// </summary>
        /// 
        private IUserService _userService;
        private ICategoryService _categoryService;
        private IArticleService _articleService;
        private ICommentService _commentService;
        private ISettingService _settionService;

        public SidebarController(IUserService userService, IArticleService articleService, ICategoryService categoryService, ICommentService commentService, ISettingService settionService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _settionService = settionService;
            _commentService = commentService;
            _userService = userService;
        }

        public ActionResult Enum()
        {
            ///新建模型
            SidebarModel sidebarModel = new SidebarModel();
            sidebarModel.MonthCategory = _categoryService.GetMonthCategory();
            sidebarModel.Categorys = _categoryService.GetAllCategory();
            sidebarModel.LastBlog = _articleService.GetLastBlog();
            sidebarModel.LastComment = _commentService.GetLastComments();

            ViewBag.SidebarModel = sidebarModel;

            Blog.Model.User user = _userService.GetLoginUser();
            ViewBag.IsLogin = user != null;
            ViewBag.IsAdmin = ViewBag.IsLogin && user.Role == Blog.Domain.UserRole.Admin;

            return View("_Sidebar", sidebarModel);
        }

    }
}
