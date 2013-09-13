using System;

namespace Blog.Model
{
	/// <summary>
	/// 文章扩展类
	/// </summary>
	public class ArticleExtend
	{
		/// <summary>
		/// 扩展ID
		/// </summary>
		public virtual int ArticleExtendId { get; set; }

		/// <summary>
		/// 文章
		/// </summary>
        public virtual Article Article { get; set; }

		/// <summary>
		/// 扩展名
		/// </summary>
		public virtual string ExtendKey { get; set; }

		/// <summary>
		/// 扩展值
		/// </summary>
		public virtual string ExtendValue { get; set; }

	}
}