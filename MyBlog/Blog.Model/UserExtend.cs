using System;

namespace Blog.Model
{
	/// <summary>
	/// 用户信息扩展类
	/// </summary>
    [Serializable]
	public class UserExtend
	{
		/// <summary>
		/// 扩展ID
		/// </summary>
		public virtual int UserExtendId { get; set; }

		/// <summary>
		/// 用户
		/// </summary>
		public virtual User User { get; set; }

		/// <summary>
		/// 扩展名称
		/// </summary>
		public virtual string ExtendKey { get; set; }

		/// <summary>
		/// 扩展值
		/// </summary>
		public virtual string ExtendValue { get; set; }

	}
}