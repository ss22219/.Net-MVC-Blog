using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blog.Model;
using Blog.Repository.Interface;
using Blog.Domain.Interface;
namespace Blog.Domain
{
    /// <summary>
    /// 权限管理类
    /// </summary>
    public class RoleService : IRoleService
    {
        private ISessionManager _sessionManager;
        private ISettingService _settingService;

        public RoleService(ISessionManager sessionManager, ISettingService settingService)
        {
            _settingService = settingService;
            _sessionManager = sessionManager;
        }
        /// <summary>
        /// 添加文章权限
        /// </summary>
        /// <returns></returns>
        public bool AddArticlePower()
        {
            return AdminPower();
        }

        /// <summary>
        /// 编辑文章权限
        /// </summary>
        /// <returns></returns>
        public bool EditArticlePower()
        {
            return AdminPower();
        }

        /// <summary>
        /// 删除文章权限
        /// </summary>
        /// <returns></returns>
        public bool DeleteArticlePower()
        {
            return AdminPower();
        }

        /// <summary>
        /// 添加评论权限
        /// </summary>
        /// <returns></returns>
        public bool AddCommentPower()
        {
            User user = _sessionManager.User;
            string CloseComment = _settingService.GetSetting("CloseComment");
            string NoLonginComment = _settingService.GetSetting("NoLoginComment");

            if (user != null && user.Role == UserRole.Admin)
            {
                return true;
            }
            else if (!CloseComment.Equals("1"))
            {
                if (user != null && user.Role != UserRole.Unverified && user.Status != UserStatus.BlackList)
                {
                    return true;
                }
                else if (user == null && NoLonginComment.Equals("1"))
                {
                    return true;
                }
            }
            return false;

        }

        /// <summary>
        /// 编辑评论权限
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public bool EditCommentPower(Comment comment)
        {
            User user = _sessionManager.User;
            if (user == null)
                return false;
            else if (user.Role == UserRole.Admin)
                return true;
            else if (user.Role != UserRole.Unverified && user.Status != UserStatus.BlackList && comment.User.UserId == user.UserId)
                return true;
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除评论权限
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public bool DeleteCommentPower(Comment comment)
        {
            return EditCommentPower(comment);
        }

        /// <summary>
        /// 登陆权限
        /// </summary>
        /// <returns></returns>
        public bool LoginPower()
        {
            return !_settingService.GetSetting("CloseSite").Equals("1");
        }

        /// <summary>
        /// 注册权限
        /// </summary>
        /// <returns></returns>
        public bool RegisterPower()
        {
            return _settingService.GetSetting("AllRegister").Equals("1");
        }

        /// <summary>
        /// 管理权限
        /// </summary>
        /// <returns></returns>
        public bool AdminPower()
        {
            User user = _sessionManager.User;
            return user != null && user.Role == UserRole.Admin;
        }

        /// <summary>
        /// 阅读文章权限
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public bool ReadArticelPower(Article article)
        {
            User user = _sessionManager.User;
            if (AdminPower())
            {
                return true;
            }
            else if (article.Status == ArticleStatus.Open)
            {
                return true;
            }
            else if (user != null && user.UserId == article.Author.UserId && article.Status != ArticleStatus.Delete)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
