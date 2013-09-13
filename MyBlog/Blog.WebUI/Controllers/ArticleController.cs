using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Model;
using Blog.Domain.Interface;

namespace Blog.WebUI.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleService _articleService;
        private IRoleService _roleService;
        private ISessionManager _sessionManager;
        private ICommentService _commentService;
        public ArticleController(IArticleService articleService, IRoleService roleService, ISessionManager sessionManager, ICommentService commentService)
        {
            _articleService = articleService;
            _roleService = roleService;
            _sessionManager = sessionManager;
            _commentService = commentService;
        }


        public ActionResult Index(int id = 0, int page = 1, int commentId = 0)
        {

            page = page <= 0 ? 1 : page;

            if (id <= 0)
            {
                ViewBag.Error = "对不起，您的访问出错了！";
                return View("Error");
            }
            try
            {
                ViewBag.IsLogin = _sessionManager.User != null;
                ViewBag.Author = Request.Cookies.Get("Author") != null ? Server.UrlDecode(Request.Cookies.Get("Author").Value) : "";
                ViewBag.Email = Request.Cookies.Get("Email") != null ? Server.UrlDecode(Request.Cookies.Get("Email").Value) : "";
                ViewBag.Url = Request.Cookies.Get("Url") != null ? Server.UrlDecode(Request.Cookies.Get("Url").Value) : "";

                Article article = _articleService.BrowseArticle(id);

                if (commentId > 0)
                {
                    Comment comment = _commentService.GetComment(commentId);
                    if (comment != null && comment.Article.ArticleId == id)
                    {
                        page = _commentService.GetCommentPageIndex(commentId);
                    }
                }

                Blog.Domain.PageInfo<Comment> comments = _commentService.GetCommentsByArticleId(article.ArticleId, page);
                ViewBag.Comments = comments;
                page = page > comments.TotalPage ? comments.TotalPage : page;
                ViewBag.Page = page;

                return View(article);
            }
            catch (Domain.DomainException ex)
            {
                ViewBag.Error = ex.Errors.First().Value;
                return View("Error");
            }
        }
    }
}
