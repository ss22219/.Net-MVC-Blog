namespace Blog.Domain.Interface
{
    /// <summary>
    /// 角色管理类 目前博客程序角色只有固定的三个 （管理员，普通用户，未激活）状态有两中（普通，黑名单）
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// 检查是否有权限阅读文章
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        bool ReadArticelPower(Blog.Model.Article article);

        /// <summary>
        /// 添加文章权限
        /// </summary>
        /// <returns></returns>
        bool AddArticlePower();

        /// <summary>
        /// 编辑文章权限
        /// </summary>
        /// <returns></returns>
        bool EditArticlePower();

        /// <summary>
        /// 删除文章权限
        /// </summary>
        /// <returns></returns>
        bool DeleteArticlePower();

        /// <summary>
        /// 添加评论权限
        /// </summary>
        /// <returns></returns>
        bool AddCommentPower();

        /// <summary>
        /// 编辑评论权限
        /// </summary>
        /// <returns></returns>
        bool EditCommentPower(Blog.Model.Comment comment);


        /// <summary>
        /// 删除评论权限
        /// </summary>
        /// <returns></returns>
        bool DeleteCommentPower(Blog.Model.Comment comment);

        /// <summary>
        /// 登录权限
        /// </summary>
        /// <returns></returns>
        bool LoginPower();

        /// <summary>
        /// 注册权限
        /// </summary>
        /// <returns></returns>
        bool RegisterPower();

        /// <summary>
        /// 管理权限
        /// </summary>
        /// <returns></returns>
        bool AdminPower();
    }
}
