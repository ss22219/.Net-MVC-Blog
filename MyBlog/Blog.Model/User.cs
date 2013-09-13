using System;
using System.Collections.Generic;
namespace Blog.Model
{
	/// <summary>
	/// 用户类
	/// </summary>
	public class User
	{
		/// <summary>
		/// 用户ID
		/// </summary>
		public virtual int UserId { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		public virtual string UserName { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		public virtual string Password { get; set; }

		/// <summary>
		/// 昵称
		/// </summary>
		public virtual string NiceName { get; set; }

		/// <summary>
		/// 邮箱
		/// </summary>
		public virtual string Email { get; set; }

		/// <summary>
		/// 注册时间
		/// </summary>
		public virtual DateTime RegisterDate { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public virtual int Role { get; set; }

		/// <summary>
		/// 用户状态
		/// </summary>
		public virtual int Status { get; set; }

        /// <summary>
        /// 用户扩展
        /// </summary>
        public virtual IDictionary<string,string> Extends { get; set; }

        /// <summary>
        /// 用户文章列表
        /// </summary>
        public virtual IList<Article> Articles { get; set; }

        /// <summary>
        /// 用户评论列表
        /// </summary>
        public virtual IList<Comment> Comments { get; set; }
	}
}