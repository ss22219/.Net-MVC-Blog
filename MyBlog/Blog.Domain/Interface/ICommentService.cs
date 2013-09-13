using Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Interface
{
    public interface ICommentService
    {
        /// <summary>
        /// 取得最新评论
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        IList<Blog.Model.Comment> GetLastComments();


        /// <summary>
        /// 取得最新评论
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        PageInfo<Blog.Model.Comment> GetCommentsByArticleId(int articleId, int pageIndexe);

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="comment"></param>
        int AddComment(Blog.Model.Comment comment);

        /// <summary>
        /// 取得评论
        /// </summary>
        /// <param name="id"></param>
        Comment GetComment(int id);


        /// <summary>
        /// 取得评论的子评论
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        IList<Comment> GetCommentsByParentId(int pid);

        /// <summary>
        /// 取得评论的所在页数
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        int GetCommentPageIndex(int commentId);

        /// <summary>
        /// 使用委托查询取评论
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        object GetCommentSingle(Func<IQueryable<Comment>, object> expr);

        /// <summary>
        /// 取得等待审核评论数量
        /// </summary>
        /// <returns></returns>
        int GetVerifyCommentCount();

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="commentId"></param>
        void DeleteComment(int commentId);

        /// <summary>
        /// 更新评论
        /// </summary>
        /// <param name="comment"></param>
        void UpdateComment(Comment comment);

        IList<Comment> Find(Func<IQueryable<Comment>, IQueryable<Comment>> expr);

        object Single(Func<IQueryable<Comment>, object> expr);
    }
}
