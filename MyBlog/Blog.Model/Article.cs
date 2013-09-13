using System;
using System.Collections.Generic;
namespace Blog.Model
{
	/// <summary>
	/// 文章类
	/// </summary>
	public class Article
	{
		/// <summary>
		/// 文章ID
		/// </summary>
		public virtual int ArticleId { get; set; }

        /// <summary>
        /// 文章作者
        /// </summary>
        public virtual User Author { get; set; }

		/// <summary>
		/// 标题
		/// </summary>
		public virtual string Title { get; set; }

		/// <summary>
		/// 内容
		/// </summary>
		public virtual string Content { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateDate { get; set; }

		/// <summary>
		/// 修改时间
		/// </summary>
		public virtual DateTime ModifyDate { get; set; }

		/// <summary>
		/// 评论数
		/// </summary>
		public virtual int CommentCount { get; set; }

	    /// <summary>
	    /// 浏览数
	    /// </summary>
		public virtual int Browse { get; set; }

		/// <summary>
		/// 文章类型
		/// </summary>
		public virtual int Type { get; set; }

		/// <summary>
		/// 文章状态
		/// </summary>
		public virtual int Status { get; set; }

        /// <summary>
        /// 文章扩展列表
        /// </summary>
        public virtual IDictionary<string,string> Extends { get; set; }

        /// <summary>
        /// 文章分类以及标签
        /// </summary>
        public virtual IList<Category> Categorys { get; set; }

        /// <summary>
        /// 文章附件列表
        /// </summary>
        public virtual IList<Attach> Attachs { get; set; }
	}
}