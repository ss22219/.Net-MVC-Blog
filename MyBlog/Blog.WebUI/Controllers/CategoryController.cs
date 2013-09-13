using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Model;
using Blog.Domain.Interface;
namespace Blog.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryService _categoryService;
        private IArticleService _articleService;
        public CategoryController(ICategoryService categoryService, IArticleService articleService)
        {
            _categoryService = categoryService;
            _articleService = articleService;
        }

        /// <summary>
        /// 取得当天的所有博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Day(string id, int page = 1)
        {
            page = page <= 0 ? 1 : page;
            ViewBag.Page = page;
            try
            {
                int year = int.Parse(id.Substring(0, 4));
                int month = int.Parse(id.Substring(4, 2));
                int day = int.Parse(id.Substring(6, 2));
                Blog.Domain.PageInfo<Article> list = _articleService.FindArticleByDay(new DateTime(year, month, day), page);

                ViewBag.Title = id.Insert(4, "年").Insert(7, "月") + "日";
                return View("Index", list);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 取得当月博客
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Month(string id, int page = 1)
        {
            page = page <= 0 ? 1 : page;
            ViewBag.Page = page;
            try
            {
                int year = int.Parse(id.Substring(0, 4));
                int month = int.Parse(id.Substring(4, 2));
                Blog.Domain.PageInfo<Article> list = _articleService.FindArticleByMonth(new DateTime(year, month, 1), page);

                ViewBag.Title = id.Insert(4, "年") + "月";
                return View("Index", list);
            }
            catch (FormatException)
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 按分类显示文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Category(int id = 0, int page = 1)
        {
            page = page <= 0 ? 1 : page;
            ViewBag.Page = page;
            Category category = _categoryService.GetCategoryById(id);
            if (category == null)
                return View("Error");
            else
            {
                ViewBag.Title = category.Name;
                Blog.Domain.PageInfo<Article> pageInfo = _articleService.GetArticleByCategory(id, page);
                return View("Index", pageInfo);
            }
        }

        /// <summary>
        /// 根据标签取得
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Tag(string id, int page = 1)
        {
            page = page <= 0 ? 1 : page;
            ViewBag.Page = page;
            Category category = _categoryService.FindTagByName(id);
            if (category == null)
                return View("Error");
            else
            {
                ViewBag.Title = category.Name;
                Blog.Domain.PageInfo<Article> list = _articleService.GetArticleByCategory(category.CategoryId, page);
                return View("Index", list);
            }
        }
    }
}
