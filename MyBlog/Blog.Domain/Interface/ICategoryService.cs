using Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Interface
{
    public interface ICategoryService
    {
        /// <summary>
        /// 取得所有分类
        /// </summary>
        /// <returns></returns>
        IList<Category> GetAllCategory();

        /// <summary>
        /// 取得按月份归档的分类<时间,显示>
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> GetMonthCategory();

        /// <summary>
        /// 根据分类ID取得分类
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Category GetCategoryById(int categoryId);

        /// <summary>
        /// 根据标签明取得标签
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Category FindTagByName(string name);

        /// <summary>
        /// 添加一个分类返回分类编号
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        int AddCategory(Category category);

        /// <summary>
        /// 取得常用的标签
        /// </summary>
        /// <returns></returns>
        IList<Category> GetLastTag();

        /// <summary>
        /// 更新一个分类
        /// </summary>
        /// <param name="category"></param>
        void UpdateCategory(Category category);

        /// <summary>
        /// 删除一个分类
        /// </summary>
        /// <param name="id"></param>
        void DeleteCaetegory(int id);
    }
}
