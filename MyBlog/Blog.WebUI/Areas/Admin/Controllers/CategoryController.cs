using Blog.Domain;
using Blog.Domain.Interface;
using Blog.Model;
using Blog.WebUI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.WebUI.Areas.Admin.Controllers
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
        //
        // GET: /Admin/Category/

        [AdminAuthorize]
        public ActionResult Index(string Message = null)
        {
            ViewBag.Message = Message;
            return View(_categoryService.GetAllCategory());
        }

        [AdminAuthorize]
        [HttpPost]
        public ActionResult Add(string name, int parentId, int type)
        {
            try
            {
                Category parent = null;
                if (parentId > 0)
                {
                    parent = _categoryService.GetCategoryById(parentId);
                }
                Category category = new Category() { Name = name, Parent = parent, Type = type };
                _categoryService.AddCategory(category);
            }
            catch (DomainException ex)
            {

                ViewBag.Message = ex.Errors.First().Value;
            }

            return RedirectToAction("Index", new { Message = ViewBag.Message });
        }

        [AdminAuthorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {

            Category category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return RedirectToAction("Index", new { Message = "该分类不存在！" });
            }

            return View(category);
        }

        [AdminAuthorize]
        [HttpPost]
        public ActionResult Edit(CategoryModel model)
        {
            Category category2 = _categoryService.GetCategoryById(model.CategoryId);
            if (category2 == null)
            {
                return RedirectToAction("Index", new { Message = "该分类不存在！" });
            }
            category2.Name = model.Name;
            category2.Parent = _categoryService.GetCategoryById(model.ParentId);
            _categoryService.UpdateCategory(category2);
            return RedirectToAction("Index", new { Message = "分类修改成功。" });
        }

        [AdminAuthorize]
        public JsonResult AjaxAdd(string name, int parentId, int articleId)
        {
            string message = null;
            try
            {
                Category parent = null;
                if (parentId > 0)
                {
                    parent = _categoryService.GetCategoryById(parentId);
                    if (parent == null)
                    {
                        throw new Exception("上级分类不存在！");
                    }
                }
                Category category = new Category() { Name = name, Type = CategoryType.Category, Parent = parent };
                _categoryService.AddCategory(category);
                Article article = articleId > 0 ? _articleService.GetArticle(articleId) : new Article();
                MvcHtmlString categoryList = CategoryHelper.BulidArticleCategory(null, article);
                return Json(new { success = true, data = categoryList.ToString(), data2 = CategoryHelper.BulidCategoryList(null).ToString() });
            }
            catch (Domain.DomainException ex)
            {
                message = ex.Errors.First().Value;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { success = false, message = message });
        }


        [AdminAuthorize]
        public ActionResult Delete(int[] CategoryIds)
        {
            string Message = "";
            try
            {
                foreach (int id in CategoryIds)
                {
                    _categoryService.DeleteCaetegory(id);
                }
                Message = CategoryIds.Length + " 个分类成功被删除。";
            }
            catch (DomainException ex)
            {
                Message = ex.Errors.First().Value;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return RedirectToAction("Index", new { Message = Message });
        }
    }
}
