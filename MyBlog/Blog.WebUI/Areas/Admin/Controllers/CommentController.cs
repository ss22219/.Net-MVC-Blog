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
    public class CommentController : Controller
    {
        private ICommentService _commentService;
        private ISettingService _settingService;
        private IArticleService _articleService;

        public CommentController(IArticleService articleService, ICommentService commentService, ISettingService settingService)
        {
            _commentService = commentService;
            _settingService = settingService;
            _articleService = articleService;
        }
        [AdminAuthorize]
        public ActionResult Index(CommentSearchModel model, string message = null, string method = null, int[] commentIds = null)
        {
            if (method != null)
            {
                if (method.Equals("Trash"))
                    return TrashList(commentIds);
                else if (method.Equals("Delete"))
                    return DeleteList(commentIds);
                else if (method.Equals("UnTrash"))
                    return UnTrashList(commentIds);
            }

            ViewBag.Message = message;
            int pageSize = int.Parse(_settingService.GetSetting("AdminCommentPageSize"));
            if (model.PageSize != null && model.PageSize > 0 && model.PageSize != pageSize)
            {
                _settingService.SetSetting("AdminCommentPageSize", model.PageSize.ToString());
            }
            else
            {
                model.PageSize = pageSize;
            }
            PageInfo<Comment> pageInfo = new PageInfo<Comment>();
            int count = (int)_commentService.Single(query => BulidCommentQuery(query, model).Count());

            pageInfo.TotalItem = count;
            pageInfo.PageSize = model.PageSize.Value;

            model.PageIndex = model.PageIndex > pageInfo.TotalPage ? pageInfo.TotalPage : model.PageIndex;

            IList<Comment> list = _commentService.Find(query => BulidCommentQuery(query, model).OrderByDescending(c => c.CreateDate).Skip((model.PageIndex - 1) * model.PageSize.Value).Take(model.PageSize.Value));
            pageInfo.PageItems = list;
            model.PageInfo = pageInfo;

            model.AllCount = (int)_commentService.Single(query => query.Where(c => (c.Status == CommentStatus.Open) || (c.Status == CommentStatus.Verify)).Count());
            model.OpenCount = (int)_commentService.Single(query => query.Where(c => (c.Status == CommentStatus.Open)).Count());
            model.VerifyCount = (int)_commentService.Single(query => query.Where(c => (c.Status == CommentStatus.Verify)).Count());
            model.DeleteCount = (int)_commentService.Single(query => query.Where(c => (c.Status == CommentStatus.Delete)).Count());


            IDictionary<int, int> VerifyComment = new Dictionary<int, int>();
            foreach (Comment comment in model.PageInfo)
            {
                if (!VerifyComment.ContainsKey(comment.Article.ArticleId))
                    VerifyComment[comment.Article.ArticleId] = (int)_commentService.GetCommentSingle(query => query.Where(c => c.Article.Type == ArticleType.Blog && c.Article.ArticleId == comment.Article.ArticleId && c.Status == CommentStatus.Verify).Count());
            }
            ViewBag.VerifyComment = VerifyComment;
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            string Message = "1 个项目成功删除。";
            try
            {
                _commentService.DeleteComment(id);
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

        public ActionResult Trash(int id)
        {
            string Message = "1 个项目成功移至回收站。";
            try
            {
                Comment comment = _commentService.GetComment(id);
                comment.Status = CommentStatus.Delete;
                _commentService.UpdateComment(comment);
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

        public ActionResult DeleteList(int[] commentIds)
        {
            string Message = commentIds.Length + " 个项目成功删除。";
            try
            {
                foreach (int id in commentIds)
                    _commentService.DeleteComment(id);
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

        public ActionResult TrashList(int[] commentIds)
        {
            string Message = commentIds.Length + " 个项目成功移至回收站。";
            try
            {
                foreach (int id in commentIds)
                {
                    Comment comment = _commentService.GetComment(id);
                    comment.Status = CommentStatus.Delete;
                    _commentService.UpdateComment(comment);
                }
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

        public ActionResult UnTrash(int id)
        {

            string Message = "1 个项目成功从回收站还原。";
            try
            {
                Comment comment = _commentService.GetComment(id);
                comment.Status = CommentStatus.Open;
                _commentService.UpdateComment(comment);
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

        public ActionResult UnTrashList(int[] commentIds)
        {

            string Message = commentIds.Length + " 个项目成功从回收站还原。";
            try
            {
                foreach (int id in commentIds)
                {
                    Comment comment = _commentService.GetComment(id);
                    comment.Status = CommentStatus.Open;
                    _commentService.UpdateComment(comment);
                }
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
        [AdminAuthorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            string Message = "";
            try
            {
                Comment comment = _commentService.GetComment(id);

                return View(comment);
            }
            catch (DomainException ex)
            {
                Message = ex.Errors.First().Value;
                return RedirectToAction("Index", new { Message = Message });
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return RedirectToAction("Index", new { Message = Message });
            }
        }

        [AdminAuthorize]
        [HttpPost]
        public ActionResult Edit(Comment comment)
        {
            string Message = "";
            try
            {
                Comment comment2 = _commentService.GetComment(comment.CommentId);
                comment2.Content = comment.Content;
                comment2.Author = comment.Author;
                comment2.AuthorMail = comment.AuthorMail;
                comment2.Status = comment.Status;
                _commentService.UpdateComment(comment2);
                return RedirectToAction("Index", new { Message = "评论编辑成功。" });
            }
            catch (DomainException ex)
            {
                Message = ex.Errors.First().Value;
                return RedirectToAction("Index", new { Message = Message });
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return RedirectToAction("Index", new { Message = Message });
            }
        }

        public ActionResult Open(int id)
        {
            string Message = "1 个项目成功批准。";
            try
            {
                Comment comment = _commentService.GetComment(id);
                comment.Status = CommentStatus.Open;
                _commentService.UpdateComment(comment);
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

        public IQueryable<Comment> BulidCommentQuery(IQueryable<Comment> query, CommentSearchModel model)
        {
            if (!string.IsNullOrEmpty(model.Search))
            {
                query = query.Where(c => c.Author.Contains(model.Search) || c.Content.Contains(model.Search));
            }

            if (!string.IsNullOrEmpty(model.IpAddress))
            {
                query = query.Where(c => c.AuthorIP == model.IpAddress);
            }

            if (model.Status == null)
            {
                query = query.Where(c => c.Status == CommentStatus.Open || c.Status == CommentStatus.Verify);
            }
            else
            {
                query = query.Where(c => c.Status == model.Status);
            }

            return query;
        }

    }
}
