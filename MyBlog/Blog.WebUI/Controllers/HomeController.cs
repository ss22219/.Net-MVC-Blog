using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Domain.Interface;
using Blog.WebUI.Models;

namespace Blog.WebUI.Controllers
{
    /// <summary>
    /// 这个好，首页哦
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 由依赖注入主键赋值
        /// </summary>
        private IArticleService _articelService;
        private ICategoryService _categoryService;

        public HomeController(IArticleService articelService, ICategoryService categoryService)
        {
            _articelService = articelService;
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            HomeModel model = new HomeModel();
            model.Categorys = _categoryService.GetAllCategory();
            model.LastBlogs = _articelService.GetLastBlog();
            model.DateCategory = _categoryService.GetMonthCategory();
            model.LastPage = _articelService.GetLastPage();
            return View(model);
        }

    }
}
