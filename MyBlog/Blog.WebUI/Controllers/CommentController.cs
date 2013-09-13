using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.WebUI.Models;
using Blog.Domain.Interface;
using Blog.Model;

namespace Blog.WebUI.Controllers
{
    public class CommentController : Controller
    {
        private ISessionManager _sessionManager;
        private ICommentService _commentService;
        private IArticleService _articleService;
        public CommentController(ISessionManager sessionManager, ICommentService commentService, IArticleService articleService)
        {
            _commentService = commentService;
            _sessionManager = sessionManager;
            _articleService = articleService;
        }

        [ValidateInput(false)]
        public ActionResult Add(CommentModel model)
        {
            int id;
            if (ModelState.IsValid)
            {
                try
                {
                    User user = _sessionManager.User;
                    Comment comment = null;
                    if (user == null)
                    {
                        comment = new Comment() { Article = _articleService.GetArticle(model.ArticleId), AuthorMail = model.Email, Content = model.Content, Parent = model.ParentId > 0 ? _commentService.GetComment(model.ParentId) : null, AuthorIP = Request.UserHostAddress, Author = model.Author };
                        DateTime timeout = DateTime.Now.AddMonths(1);

                        Response.Cookies.Add(new HttpCookie("Author", Server.UrlEncode(model.Author)) { Expires = timeout });
                        Response.Cookies.Add(new HttpCookie("Email", Server.UrlEncode(model.Email)) { Expires = timeout });
                        Response.Cookies.Add(new HttpCookie("Url", Server.UrlEncode(model.Url)) { Expires = timeout });
                    }
                    else
                    {
                        comment = new Comment() { Article = _articleService.GetArticle(model.ArticleId), AuthorMail = user.Email, Content = model.Content, Parent = model.ParentId > 0 ? _commentService.GetComment(model.ParentId) : null, AuthorIP = Request.UserHostAddress, Author = user.UserName };
                    }
                    id = _commentService.AddComment(comment);
                }
                catch (Domain.DomainException ex)
                {
                    ViewBag.Error = ex.Errors.First().Value;
                    return View("Error");
                }
            }
            else
            {
                bool s = false;
                foreach (ModelState state in ModelState.Values)
                {
                    foreach (ModelError error in state.Errors)
                    {
                        ViewBag.Error = error.ErrorMessage;
                        s = true;
                        break;
                    }
                    if (s)
                    {
                        break;
                    }
                }
                return View("Error");
            }
            return Redirect("/Article/" + model.ArticleId + "/" + _commentService.GetCommentPageIndex(id) + "#comment-" + id);
        }

    }
}
