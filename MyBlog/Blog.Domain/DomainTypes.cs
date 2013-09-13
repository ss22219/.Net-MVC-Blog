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
        static ArticleStatus()
        {
            Names[Temp] = "草稿";
            Names[Open] = "已发布";
            Names[Delete] = "回收站";
        }

        public static IDictionary<int, string> Names = new Dictionary<int, string>();

        /// <summary>
        /// 草稿
        /// </summary>
        public static readonly int Temp = 0;
        /// <summary>
        /// 公开
        /// </summary>
        public static readonly int Open = 1;
        /// <summary>
        /// 回收站
        /// </summary>
        public static readonly int Delete = 2;
    }
    /// <summary>
    /// 附件类型
    /// </summary>
    public static class AttachType
    {
        /// <summary>
        /// 图片
        /// </summary>
        public static readonly int Image = 0;
        /// <summary>
        /// 其他
        /// </summary>
        public static readonly int Other = 1;
    }
    /// <summary>
    /// 分类类型
    /// </summary>
    public static class CategoryType
    {
        /// <summary>
        /// 类别
        /// </summary>
        public static readonly int Category = 0;
        /// <summary>
        /// 标签
        /// </summary>
        public static readonly int Tag = 1;

        /// <summary>
        /// 相册
        /// </summary>
        public static readonly int Album = 2;
    }

    /// <summary>
    /// 评论状态
    /// </summary>
    public static class CommentStatus
    {
        /// <summary>
        /// 等待审核
        /// </summary>
        public static readonly int Verify = 0;
        /// <summary>
        /// 公开
        /// </summary>
        public static readonly int Open = 1;
        /// <summary>
        /// 回收站
        /// </summary>
        public static readonly int Delete = 2;
    }
    /// <summary>
    /// 用户角色
    /// </summary>
    public static class UserRole
    {
        /// <summary>
        /// 未通过审核
        /// </summary>
        public static readonly int Unverified = 0;
        /// <summary>
        /// 读者
        /// </summary>
        public static readonly int Reader = 1;
        /// <summary>
        /// 管理员
        /// </summary>
        public static readonly int Admin = 2;

        public static readonly string[] Names = new string[] { "审核", "读者", "管理员" };
    }

    /// <summary>
    /// 用户状态
    /// </summary>
    public static class UserStatus
    {
        /// <summary>
        /// 普通
        /// </summary>
        public static readonly int Normal = 0;
        /// <summary>
        /// 黑名单
        /// </summary>
        public static readonly int BlackList = 1;
    }
}
