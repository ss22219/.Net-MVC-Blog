using System;
using System.Collections.Generic;
namespace Blog.Model
{
	/// <summary>
	/// 分类表
	/// </summary>
	public class Category
	{
		/// <summary>
		/// 分类ID
		/// </summary>
		public virtual int CategoryId { get; set; }

		/// <summary>
		/// 分类名称
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// 分类类型
		/// </summary>
		public virtual int Type { get; set; }

		/// <summary>
		/// 父分类
		/// </summary>
        public virtual Category Parent { get; set; }

        /// <summary>
        /// 子分类
        /// </summary>
        public virtual IList<Category> Children { get; set; }

		/// <summary>
		/// 文章数
		/// </summary>
		public virtual int Count { get; set; }

	}
}