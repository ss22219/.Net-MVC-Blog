using System;
namespace Blog.Model
{
	/// <summary>
	/// 附件表
	/// </summary>
	public class Attach
	{
		/// <summary>
		/// 附件ID
		/// </summary>
		public virtual int AttachId { get; set; }

        /// <summary>
        /// 文章
        /// </summary>
        public virtual Article Article { get; set; }

		/// <summary>
		/// 标题
		/// </summary>
		public virtual string Title { get; set; }

		/// <summary>
		/// 保存路径
		/// </summary>
		public virtual string Path { get; set; }

		/// <summary>
		/// 附件类型
		/// </summary>
		public virtual int Type { get; set; }
	}
}