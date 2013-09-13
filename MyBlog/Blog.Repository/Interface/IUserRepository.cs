using System;
using System.Collections.Generic;
using Blog.Model;
namespace Blog.Repository.Interface
{
    /// <summary>
    /// 用户数据操作
    /// </summary>
    public interface IUserRepository:IRepositoryBase<User>
    {
        /// <summary>
        /// 用户名是否存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        bool CheckUserNameExist(string userName);

        /// <summary>
        /// 检查用户信息是否匹配
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">加密后的密码</param>
        /// <returns></returns>
        User CheckUser(string userName, string password);

        /// <summary>
        /// 检查邮箱是否已经被注册
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        bool CheckEmailExist(string email);
    }
}
