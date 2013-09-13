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
    public class ArticleController : Controller
    {
        private IArticleService _articleService;
        private IRoleService _roleService;
        public ICommentService _commentService;
        private ICategoryService _categoryService;
        private IUserService _userSrvice;
        private ISettingService _settiongService;

        public ArticleController(IUserService userSerivce, IRoleService roleService, IArticleService articleService, ICommentService commentService, ICategoryService categoryService, ISettingService settiongService)
        {
            _userSrvice = userSerivce;
            _roleService = roleService;
            _articleService = articleService;
            _commentService = commentService;
            _categoryService = categoryService;
            _settiongService = settiongService;
        }

        [AdminAuthorize]
        public ActionResult Index(ArticleQueryModel model, string Message = null)
        {
            ///操作判断开始
            if (!string.IsNullOrEmpty(model.Method))
            {
                if (model.Method.Equals("Trash"))
                {
                    return TrashList(model.ArticleIds);
                }
                else if (model.Method.Equals("Edit"))
                {
                    //return Edit(model.ArticleIds);
                }
                else if (model.Method.Equals("UnTrash"))
                {
                    return UnTrashList(model.ArticleIds);
                }
                else if (model.Method.Equals("Delete"))
                {
                    return DeleteList(model.ArticleIds);
                }
            }
            ViewBag.Message = Message;
            ///取得每页显示数
            int pageSize = int.Parse(_settiongService.GetSetting("AdminArticlePageSize"));
            model.Page = model.Page <= 0 ? 1 : model.Page;
            model.PageSize = model.PageSize <= 0 ? pageSize : model.PageSize;

            ///更新数据库
            if (model.PageSize != pageSize)
            {
                _settiongService.SetSetting("AdminArticlePageSize", model.PageSize.ToString());
            }

            //构造查询条件
            model.PageInfo = _articleService.FindArticlesByQuery(query => BulidQuery(query, model), model.Page, model.PageSize);
            model.Page = model.PageInfo.PageIndex;


            ///取得博客数目
            model.AllCount = (int)_articleService.GetArticleSingle(qyery => qyery.Where(a => a.Type == ArticleType.Blog).Count());
            model.OpenCount = (int)_articleService.GetArticleSingle(qyery => qyery.Where(a => a.Type == ArticleType.Blog && a.Status == ArticleStatus.Open).Count());
            model.DeleteCount = (int)_articleService.GetArticleSingle(qyery => qyery.Where(a => a.Type == ArticleType.Blog && a.Status == ArticleStatus.Delete).Count());

            ///取得审核评论数目
            IDictionary<int, int> VerifyComment = new Dictionary<int, int>();
            foreach (Article articel in model.PageInfo)
            {
                VerifyComment[articel.ArticleId] = (int)_commentService.GetCommentSingle(query => query.Where(c => c.Article.Type == ArticleType.Blog && c.Article.ArticleId == articel.ArticleId && c.Status == CommentStatus.Verify).Count());
            }
            model.VerifyComment = VerifyComment;

            ///取得分类与分档
            model.MonthCategorys = _categoryService.GetMonthCategory();
            model.Categorys = _categoryService.GetAllCategory();

            return View("Index", model);
        }

        [AdminAuthorize]
        public ActionResult Trash(int id)
        {
            try
            {
                Article article = _articleService.GetArticle(id);
                article.Status = ArticleStatus.Delete;
                _articleService.UpdateArticle(article);
                ViewBag.Message = "1 个项目已移至回收站。";
            }
            catch (DomainException ex)
            {
                ViewBag.Error = ex.Errors.First().Value;
                return View("Error");
            }
            string Message = "1 个项目已移至回收站。";
            return RedirectToAction("Index", new { Status = 2, Message = Message });
        }

        [AdminAuthorize]
        public ActionResult TrashList(int[] ArticleIds)
        {
            try
            {
                foreach (int id in ArticleIds)
                {
                    Article article = _articleService.GetArticle(id);
                    article.Status = ArticleStatus.Delete;
                    _articleService.UpdateArticle(article);
                }
            }
            catch (DomainException ex)
            {
                ViewBag.Error = ex.Errors.First().Value;
                return View("Error");
            }
            string Message = ArticleIds.Length + " 个项目已移至回收站。";
            return RedirectToAction("Index", new { Status = 2, Message = Message });
        }

        [AdminAuthorize]
        public ActionResult UnTrashList(int[] ArticleIds)
        {
            try
            {
                foreach (int id in ArticleIds)
                {
                    Article article = _articleService.GetArticle(id);
                    article.Status = ArticleStatus.Open;
                    _articleService.UpdateArticle(article);
                }
            }
            catch (DomainException ex)
            {
                ViewBag.Error = ex.Errors.First().Value;
                return View("Error");
            }
            string Message = ArticleIds.Length + " 个项目已从回收站中恢复。";
            return RedirectToAction("Index", new { Message = Message });
        }

        [AdminAuthorize]
        [ValidateInput(false)]
        public JsonResult AjaxUpdate(ArticleAjaxModel model)
        {
            try
            {
                Article old = _articleService.GetArticle(model.ArticleId);
                if (!string.IsNullOrEmpty(model.Title))
                {
                    old.Title = model.Title;
                }
                if (model.Categorys != null)
                {
                    if (model.Categorys.Length == 1 && model.Categorys[0] == 0)
                    {
                        IList<Category> list = old.Categorys.Where(c => c.Type == CategoryType.Category).ToList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            old.Categorys.Remove(list[i]);
                        }
                    }
                    else
                    {
                        foreach (int categoryId in model.Categorys)
                        {
                            if (old.Categorys.Where(c => c.CategoryId == categoryId).Count() == 0)
                            {
                                Category cat = _categoryService.GetCategoryById(categoryId);
                                cat.Count++;
                                _categoryService.UpdateCategory(cat);
                            }
                        }

                        IList<Category> list = old.Categorys.Where(c => c.Type == CategoryType.Category).ToList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            old.Categorys.Remove(list[i]);
                        }

                        foreach (int categoryId in model.Categorys)
                        {
                            old.Categorys.Add(_categoryService.GetCategoryById(categoryId));
                        }
                    }
                }
                if (!string.IsNullOrEmpty(model.Content))
                {
                    model.Content = model.Content.Replace("../../../Scripts/", "/Scripts/");
                    old.Content = model.Content;
                }
                if (model.Tags != null)
                {
                    if (string.IsNullOrEmpty(model.Tags[0]))
                    {
                        IList<Category> list = old.Categorys.Where(c => c.Type == CategoryType.Tag).ToList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            old.Categorys.Remove(list[i]);
                        }
                    }
                    else
                    {
                        foreach (string tag in model.Tags)
                        {
                            Category category = _categoryService.FindTagByName(tag);
                            if (category == null)
                            {
                                category = new Category() { Name = tag, Count = 0, Type = CategoryType.Tag };
                                _categoryService.AddCategory(category);
                            }
                            if (old.Categorys.Where(t => t.Name == tag && t.Type == CategoryType.Tag).Count() == 0)
                            {
                                category.Count++;
                                _categoryService.UpdateCategory(category);
                            }
                        }

                        IList<Category> list = old.Categorys.Where(c => c.Type == CategoryType.Tag).ToList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            old.Categorys.Remove(list[i]);
                        }

                        foreach (string tag in model.Tags)
                        {
                            Category category = _categoryService.FindTagByName(tag);
                            old.Categorys.Add(category);
                        }
                    }
                }
                if (model.Status != null)
                {
                    old.Status = model.Status.Value;
                    ViewBag.Message = ArticleStatus.Names[model.Status.Value];
                }
                if (model.CreateDate != null)
                {
                    old.CreateDate = Convert.ToDateTime(model.CreateDate);
                    ViewBag.Message = old.CreateDate.ToString("yyyy 年 M 月 d 日 h:mm");
                }
                _articleService.UpdateArticle(old);
                ViewBag.Error = null;
            }
            catch (DomainException ex)
            {
                ViewBag.Error = ex.Errors.First().Value;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

            }
            return Json(new { success = ViewBag.Error == null ? true : false, message = ViewBag.Error == null ? ViewBag.Message : ViewBag.Error });
        }

        [AdminAuthorize]
        public ActionResult UnTrash(int id)
        {
            try
            {
                Article article = _articleService.GetArticle(id);
                article.Status = ArticleStatus.Open;
                _articleService.UpdateArticle(article);
            }
            catch (DomainException ex)
            {
                ViewBag.Error = ex.Errors.First().Value;
                return View("Error");
            }
            string Message = "1 个项目已从回收站中恢复。";
            return RedirectToAction("Index", new { Message = Message });
        }

        [AdminAuthorize]
        public ActionResult DeleteList(int[] ArticleIds)
        {
            try
            {
                foreach (int id in ArticleIds)
                {
                    _articleService.DeleteArticle(id);
                }
            }
            catch (DomainException ex)
            {
                ViewBag.Error = ex.Errors.First().Value;
                return View("Error");
            }
            string Message = ArticleIds.Length + " 个项目已删除。";
            return RedirectToAction("Index", new { Message = Message });
        }

        [AdminAuthorize]
        public ActionResult Delete(int id)
        {
            try
            {
                _articleService.DeleteArticle(id);
            }
            catch (DomainException ex)
            {
                ViewBag.Error = ex.Errors.First().Value;
                return View("Error");
            }
            string Message = "1 个项目已删除。";
            return RedirectToAction("Index", new { Message = Message });

        }

        [AdminAuthorize]
        public ActionResult Edit(int id)
        {
            Article article = _articleService.GetArticle(id);
            IList<Category> lastTag = _categoryService.GetLastTag();
            ViewBag.LastTag = lastTag;
            return View(article);
        }

        [AdminAuthorize]
        [HttpGet]
        public ActionResult Add()
        {
            Article article = new Article() { Author = _userSrvice.GetLoginUser(), Content = "", Title = "", Status = ArticleStatus.Temp, CreateDate = DateTime.Now, Type = ArticleType.Blog, ModifyDate = DateTime.Now };
            IList<Category> lastTag = _categoryService.GetLastTag();
            ViewBag.LastTag = lastTag;
            return View(article);
        }

        [AdminAuthorize]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AjaxAdd(ArticleAjaxModel model)
        {
            bool success = false;
            string message = null;
            object data = null;
            try
            {
                if (ModelState.IsValid)
                {
                    Article article = new Article() { Author = _userSrvice.GetLoginUser(), Content = model.Content, Title = model.Title, Status = ArticleStatus.Temp, CreateDate = DateTime.Now, Type = ArticleType.Blog, ModifyDate = DateTime.Now, Categorys = new List<Category>() };

                    if (model.Categorys != null)
                    {
                        foreach (int categoryId in model.Categorys)
                        {
                            Category cat = _categoryService.GetCategoryById(categoryId);
                            if (cat == null)
                            {
                                throw new Exception("不存在该分类！");
                            }
                            cat.Count++;
                            _categoryService.UpdateCategory(cat);
                            article.Categorys.Add(cat);
                        }
                    }
                    if (model.Tags != null)
                    {

                        foreach (string tag in model.Tags)
                        {
                            Category category = _categoryService.FindTagByName(tag);
                            if (category == null)
                            {
                                category = new Category() { Name = tag, Count = 1, Type = CategoryType.Tag };
                                _categoryService.AddCategory(category);
                            }
                            else
                            {
                                category.Count++;
                                _categoryService.UpdateCategory(category);
                            }
                            article.Categorys.Add(category);
                        }
                    }
                    data = _articleService.AddArticle(article);
                    success = true;
                }
                else
                {
                    foreach (var value in ModelState.Values)
                    {
                        foreach (var error in value.Errors)
                        {
                            message = error.ErrorMessage;
                            break;
                        }
                        if (message != null)
                        {
                            break;
                        }
                    }

                }
            }
            catch (DomainException ex)
            {
                message = ex.Errors.First().Value;
            }
            catch (Exception ex)
            {
                message = ex.Message;

            }
            return Json(new { success = success, message = message, data = data });
        }
        /// <summary>
        /// 创建查询表达式
        /// </summary>
        /// <param name="query"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public IQueryable<Article> BulidQuery(IQueryable<Article> query, ArticleQueryModel model)
        {
            ///状态限制
            if (model.Status != null)
            {
                query = query.Where(a => a.Status == model.Status);
            }
            else
            {
                query = query.Where(a => a.Status != ArticleStatus.Delete);
            }

            ///标题查询
            if (!string.IsNullOrEmpty(model.Title))
            {
                query = query.Where(a => a.Title.Contains(model.Title));
            }

            ///分类查询
            if (model.Category != null && model.Category > 0)
            {
                query = query.Where(a => a.Categorys.Where(c => c.CategoryId == model.Category).Count() > 0);
            }

            ///标签查询
            if (!string.IsNullOrEmpty(model.Tag))
            {
                query = query.Where(a => a.Categorys.Where(c => c.Name == model.Tag && c.Type == CategoryType.Tag).Count() > 0);
            }


            ///分档查询
            if (!string.IsNullOrEmpty(model.MonthCategory))
            {
                try
                {
                    int year = int.Parse(model.MonthCategory.Substring(0, 4));
                    int month = int.Parse(model.MonthCategory.Substring(4, 2));
                    query = query.Where(a => a.CreateDate.Year == year && a.CreateDate.Month == month);
                }
                catch (Exception)
                {
                    View("Error");
                }
            }

            ///开始排序

            if (model.OrderBy == ArticleOrder.Title)
            {
                if (model.OrderType == OrderType.Asc)
                {
                    return query.OrderBy(a => a.Title);
                }
                else
                {
                    return query.OrderByDescending(a => a.Title);
                }
            }
            else if (model.OrderBy == ArticleOrder.CommentCount)
            {
                if (model.OrderType == OrderType.Asc)
                {
                    return query.OrderBy(a => a.CommentCount);
                }
                else
                {
                    return query.OrderByDescending(a => a.CommentCount);
                }
            }
            else if (model.OrderBy == ArticleOrder.CreateDate)
            {
                if (model.OrderType == OrderType.Asc)
                {
                    return query.OrderBy(a => a.CreateDate);
                }
                else
                {
                    return query.OrderByDescending(a => a.CreateDate);
                }
            }
            else
            {
                return query.OrderByDescending(a => a.CreateDate);
            }

        }

    }
}
