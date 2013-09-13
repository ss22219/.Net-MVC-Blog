using Blog.Model;

namespace Blog.Domain.Interface
{
    /// <summary>
    /// 该类是回话管理接口，用于管理用户状态，该类不在本项目内实现，由客户端实现
    /// </summary>
    public interface ISessionManager
    {
        /// <summary>
        /// 用户状态
        /// </summary>
        User User { get; set; }

        /// <summary>
        /// 取得对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object Get(string name);

        /// <summary>
        /// 设置对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void Set(string name, object value);
    }
}