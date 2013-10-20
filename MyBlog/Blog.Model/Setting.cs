using System;

namespace Blog.Model
{
	/// <summary>
	/// 设置类
    /// </summary>
    [Serializable]
	public class Setting
	{
		/// <summary>
		/// 设置ID（主键）
		/// </summary>
		public virtual int SettingId { get; set; }

		/// <summary>
		/// 设置名称
		/// </summary>
		public virtual string SettingKey { get; set; }

		/// <summary>
		/// 设置值
		/// </summary>
		public virtual string SettingValue { get; set; }

	}
}