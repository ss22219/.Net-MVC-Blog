using System;
using NHibernate;

namespace Blog.Repository.Interface
{
    /// <summary>
    /// NHibernate会话管理类（为每一个HTTP请求生成唯一的Session，并其在结束后释放该Session）
    /// </summary>
    public interface INHibernateSessionManager
    {
        /// <summary>
        /// 取得会话
        /// </summary>
        /// <returns></returns>
        ISession GetInstance();

        /// <summary>
        /// 释放会话
        /// </summary>
        void Dispose();
    }
}
