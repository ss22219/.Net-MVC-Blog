using System;

namespace Blog.Model
{
	/// <summary>
	/// 文章分类对应表
	/// </summary>
	public class CategoryRelationShip
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		public virtual int RelationShipId { get; set; }

		/// <summary>
		/// 分类ID
		/// </summary>
		public virtual int CategoryId { get; set; }

		/// <summary>
		/// 文章ID
		/// </summary>
		public virtual int ArticleId { get; set; }

	}
}