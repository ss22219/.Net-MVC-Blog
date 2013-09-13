using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Domain.Interface;
using Blog.Repository.Interface;
using Blog.Model;
using System.Web;
using System.Web.Caching;
namespace Blog.Domain
{
    /// <summary>
    /// 评论服务实现
    /// </summary>
    public class CommentService : ICommentService
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private ICommentRepository _commentRepository;
        private IArticleService _articleService;
        private ISessionManager _sessionManager;
        private IRoleService _roleService;
        private ISettingService _settiongService;

        public CommentService(IArticleService articleService, ICommentRepository commentRepository, ISessionManager sessionManger, IRoleService roleService, ISettingService settiongService)
        {
            _articleService = articleService;
            _commentRepository = commentRepository;
            _sessionManager = sessionManger;
            _roleService = roleService;
            _settiongService = settiongService;
        }

        /// <summary>
        /// 取得最新评论
        /// </summary>
        /// <returns></returns>
        public IList<Model.Comment> GetLastComments()
        {
            //缓存加载
            IList<Comment> comments = (IList<Comment>)HttpRuntime.Cache.Get("Comment_LastComment");

            if (comments != null)
            {
                return comments;
            }
            int count = int.Parse(_settiongService.GetSetting("LastCommentCount"));
            comments = _commentRepository.Find(
                query => query.Where(
                    c => c.Status == CommentStatus.Open
                    )
                    .OrderByDescending(c => c.CreateDate).Take(count)
                );

            //加入缓存
            HttpRuntime.Cache.Insert("Comment_LastComment", comments, new SqlCacheDependency("Default", "Comment"), DateTime.UtcNow.AddMinutes(1), TimeSpan.Zero);
            return comments;
        }

        /// <summary>
        /// 取得一个文章的评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public PageInfo<Comment> GetCommentsByArticleId(int articleId, int pageIndex)
        {
            //缓存加载
            PageInfo<Comment> page = (PageInfo<Comment>)HttpRuntime.Cache.Get("Comment_ArticlePage" + articleId + "_" + pageIndex);

            if (page != null)
            {
                return page;
            }

            page = new PageInfo<Comment>();
            int userId = _sessionManager.User == null ? 0 : _sessionManager.User.UserId;
            int pageSize = int.Parse(_settiongService.GetSetting("CommentPageSize"));
            page.PageSize = pageSize;

            page.TotalItem = (int)_commentRepository.Single(query => query.Where(
                c => c.Article.ArticleId == articleId && (c.Parent.CommentId == 0 || c.Parent == null) && ((c.Status == CommentStatus.Open) || (c.User.UserId == userId && c.Status != CommentStatus.Delete))
                ).Count()
            );

            pageIndex = pageIndex > page.TotalPage ? page.TotalPage : pageIndex;

            page.PageItems = _commentRepository.Find(query => query.Where(
                c => c.Article.ArticleId == articleId && (c.Parent.CommentId == 0 || c.Parent == null) && ((c.Status == CommentStatus.Open) || (c.User.UserId == userId && c.Status != CommentStatus.Delete))
                ).OrderBy(c => c.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize)
            );


            page.PageIndex = pageIndex;

            ///加入缓存
            HttpRuntime.Cache.Insert("Comment_ArticlePage" + articleId + "_" + pageIndex, page, new SqlCacheDependency("Default", "Comment"), DateTime.UtcNow.AddMilliseconds(30), TimeSpan.Zero);
            return page;
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public int AddComment(Comment comment)
        {
            if (!_roleService.AddCommentPower())
            {
                throw new DomainException("NoPower", "对不起，您没有权限发表评论。");
            }
            else
            {
                if (_sessionManager.User == null)
                {
                    if (string.IsNullOrEmpty(comment.Author))
                    {
                        throw new DomainException("NoAuthor", "对不起，需要填写昵称才能发表评论。");
                    }
                    else if (string.IsNullOrEmpty(comment.AuthorMail))
                    {
                        throw new DomainException("NoAuthor", "对不起，需要填写邮箱才能发表评论。");
                    }
                }
                else
                {
                    comment.User = _sessionManager.User;
                    comment.Author = comment.User.UserName;
                    comment.AuthorMail = comment.User.Email;
                }
                int id = 0;
                try
                {
                    //打开事务
                    _commentRepository.BeginTransaction();

                    //取得评论所在文章
                    comment.Article = _articleService.GetArticle(comment.Article.ArticleId);

                    //如果父评论不为空，而且父评论的文章ID跟当前的文章ID不一致，抛出异常
                    if (comment.Parent != null && comment.Article.ArticleId != comment.Parent.Article.ArticleId)
                    {
                        throw new DomainException("DataError", "对不起，您提交的数据异常！");
                    }
                    //检查镶套层数
                    CheckCommentLevel(comment);

                    //给文章添加评论个数
                    comment.Article.CommentCount++;

                    //评论的创建时间
                    comment.CreateDate = DateTime.Now;

                    //评论默认状态
                    if (_roleService.AddArticlePower())
                    {
                        comment.Status = CommentStatus.Open;
                    }
                    else
                    {
                        comment.Status = int.Parse(_settiongService.GetSetting("CommentSatus"));
                    }

                    //添加文章
                    id = _commentRepository.Add(comment);

                    //事务提交
                    _commentRepository.CommitTransaction();
                }
                catch (Exception ex)
                {
                    //回滚事务
                    _commentRepository.RollbackTransaction();
                    throw ex;
                }
                return id;
            }
        }

        /// <summary>
        /// 取得父评论下的子评论
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public IList<Comment> GetCommentsByParentId(int pid)
        {
            return _commentRepository.Find(query => query.Where(c => c.Parent.CommentId == pid));
        }

        /// <summary>
        /// 取得某一个评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Comment GetComment(int id)
        {
            Comment comment = _commentRepository.Get(id);
            if (comment == null)
            {
                throw new DomainException("NoFind", "没有找到该评论！");
            }
            return comment;
        }


        /// <summary>
        /// 取得评论的所在页数。  队长：“注意，前方高能！”
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public int GetCommentPageIndex(int commentId)
        {
            //缓存开启
            int? pageIndex = (int?)HttpRuntime.Cache.Get("Comment_PageIndex" + commentId);

            if (pageIndex != null)
            {
                return pageIndex.Value;
            }
            Comment comment = _commentRepository.Get(commentId);

            while (comment.Parent != null)
            {
                comment = comment.Parent;
            }

            int count = (int)_commentRepository.Single(
                query => query
                .Where(c => (c.Article.ArticleId == comment.Article.ArticleId) && (c.CreateDate <= comment.CreateDate) && (c.Parent.CommentId == 0 || c.Parent == null))
                .Count()
            );
            int pageSize = int.Parse(_settiongService.GetSetting("CommentPageSize"));

            pageIndex = (int)Math.Ceiling(count / (double)pageSize);

            //加入缓存
            HttpRuntime.Cache.Insert("Comment_PageIndex" + commentId, pageIndex, new SqlCacheDependency("Default", "Comment"), DateTime.MaxValue, TimeSpan.FromMinutes(1));
            return pageIndex.Value;
        }

        /// <summary>
        /// 检查评论层数是否过多
        /// </summary>
        /// <param name="comment"></param>
        private void CheckCommentLevel(Comment comment)
        {
            int i = 0;
            int maxReComment = int.Parse(_settiongService.GetSetting("MaxReComment"));
            Comment parent = comment.Parent;

            while (true)
            {
                if (parent == null || parent.Parent == null)
                {
                    break;
                }
                parent = parent.Parent;
                i++;

                if (i >= maxReComment)
                {
                    throw new DomainException("ReComment", "抱歉，评论最多只能镶套" + maxReComment + "层。");
                }
            }
        }


        public object GetCommentSingle(Func<IQueryable<Comment>, object> expr)
        {
            if (_roleService.AdminPower())
            {
                new DomainException("NoPower", "对不起，没有权限！");
            }
            return _commentRepository.Single(expr);
        }


        public int GetVerifyCommentCount()
        {
            return (int)_commentRepository.Single(query => query.Where(c => c.Status == CommentStatus.Verify).Count());
        }


        public IList<Comment> Find(Func<IQueryable<Comment>, IQueryable<Comment>> expr)
        {
            return _commentRepository.Find(expr);
        }

        public object Single(Func<IQueryable<Comment>, object> expr)
        {
            return _commentRepository.Single(expr);
        }


        public void DeleteComment(int commentId)
        {
            Comment comment = GetComment(commentId);
            Article article = _articleService.GetArticle(comment.Article.ArticleId);
            article.CommentCount--;
            _articleService.UpdateArticle(article);
            _commentRepository.Delete(comment.CommentId);
        }


        public void UpdateComment(Comment comment)
        {
            _commentRepository.Update(comment);
        }
    }

}
