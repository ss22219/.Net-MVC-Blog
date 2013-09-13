using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blog.Model;

namespace Blog.Domain.Interface
{
    /// <summary>
    /// 用户业务类
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 取得当前登录用户
        /// </summary>
        /// <returns></returns>
        User GetLoginUser();

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="Password"></param>
        /// <param name="remember"></param>
        /// <returns></returns>
        User Login(string userName, string Password);

        /// <summary>
        /// 登出
        /// </summary>
        void LogOut();

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        void Register(User user);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user">需要修改密码的用户</param>
        /// <param name="oldPasswrod">旧密码</param>
        /// <param name="newPassword">新密码</param>
        void RePassword(User user, string oldPasswrod, string newPassword);

        /// <summary>
        /// 发送新密码到邮箱
        /// </summary>
        /// <param name="UserName"></param>
        void SendNewPassword(string UserName);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(User user);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        void AddUser(User user);

        object Single(Func<IQueryable<User>, object> func);

        IList<User> Find(Func<IQueryable<User>, IQueryable<User>> func);

        void Delete(int id);

        User GetUserById(int id);
    }
}
