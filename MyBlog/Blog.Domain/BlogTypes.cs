using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    /// <summary>
    /// 文章类型
    /// </summary>
    public static class ArticleType
    {
        /// <summary>
        /// 博客
        /// </summary>
        public static int Blog = 0;
        /// <summary>
        /// 页面
        /// </summary>
        public static int Page = 1;
    }

    /// <summary>
    /// 文章状态
    /// </summary>
    public static class ArticleStatus
    {
        /// <summary>
        /// 草稿
        /// </summary>
        public static int Temp = 0;
        /// <summary>
        /// 公开
        /// </summary>
        public static int Open = 1;
        /// <summary>
        /// 回收站
        /// </summary>
        public static int Delete = 2;
    }
    /// <summary>
    /// 附件类型
    /// </summary>
    public static class AttachType
    {
        /// <summary>
        /// 图片
        /// </summary>
        public static int Image = 0;
        /// <summary>
        /// 其他
        /// </summary>
        public static int Other = 1;
    }
    /// <summary>
    /// 分类类型
    /// </summary>
    public static class CategoryType
    {
        /// <summary>
        /// 类别
        /// </summary>
        public static int Category = 0;
        /// <summary>
        /// 标签
        /// </summary>
        public static int Tag = 1;

        /// <summary>
        /// 相册
        /// </summary>
        public static int Album = 2;
    }
    /// <summary>
    /// 评论类型
    /// </summary>
    public static class CommentType
    {
        /// <summary>
        /// 等待审核
        /// </summary>
        public static int Verify = 0;
        /// <summary>
        /// 公开
        /// </summary>
        public static int Open = 1;
        /// <summary>
        /// 未通过审核(垃圾评论)
        /// </summary>
        public static int Unverified = 2;
    }


    public static class CommentStatus
    {
        public static int Normal = 0;
        /// <summary>
        /// 回收站
        /// </summary>
        public static int Delete = 1;
    }
    /// <summary>
    /// 用户角色
    /// </summary>
    public static class UserRole
    {
        /// <summary>
        /// 未通过审核
        /// </summary>
        public static int Unverified = 0;
        /// <summary>
        /// 读者
        /// </summary>
        public static int Reader = 1;
        /// <summary>
        /// 管理员
        /// </summary>
        public static int Admin = 2;
    }

    /// <summary>
    /// 用户状态
    /// </summary>
    public static class UserStatus
    {
        /// <summary>
        /// 普通
        /// </summary>
        public static int Normal = 0;
        /// <summary>
        /// 黑名单
        /// </summary>
        public static int BlackList = 1;
    }
}
