using System;
using System.Collections.Generic;

namespace Blog.Model
{
    /// <summary>
    /// 评论类
    /// </summary>
    [Serializable]
    public class Comment
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public virtual int CommentId { get; set; }

        /// <summary>
        /// 文章
        /// </summary>
        public virtual Article Article { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 评论作者
        /// </summary>
        public virtual string Author { get; set; }

        /// <summary>
        /// 评论者邮箱
        /// </summary>
        public virtual string AuthorMail { get; set; }

        /// <summary>
        /// 评论者IP地址
        /// </summary>
        public virtual string AuthorIP { get; set; }

        /// <summary>
        /// 评论状态
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 父级评论
        /// </summary>
        public virtual Comment Parent { get; set; }

        /// <summary>
        /// 子级评论
        /// </summary>
        public virtual IList<Comment> Children { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public virtual User User { get; set; }

    }
}