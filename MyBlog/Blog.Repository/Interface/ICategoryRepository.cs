using System;
using Blog.Model;

namespace Blog.Repository.Interface
{
    /// <summary>
    ///分类数据操作
    /// </summary>
    public interface ICategoryRepository:IRepositoryBase<Category>
    {
        System.Collections.Generic.IList<string> GetMonthCategory();
    }
}
