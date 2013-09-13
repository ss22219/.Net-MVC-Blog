using System;
using System.Collections.Generic;
using Blog.Model;

namespace Blog.Repository.Interface
{
    /// <summary>
    /// 文章数据操作
    /// </summary>
    public interface IArticleRepository : IRepositoryBase<Article>
    {
    }
}
