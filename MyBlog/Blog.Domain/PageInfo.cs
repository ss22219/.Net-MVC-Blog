using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class PageInfo<T> : IEnumerable<T>
    {
        /// <summary>
        /// 每页显示个数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 最大个数
        /// </summary>
        public int TotalItem { get; set; }

        /// <summary>
        /// 最大页数
        /// </summary>
        public int TotalPage { get { return (int)Math.Ceiling((decimal)TotalItem / (decimal)PageSize); } }

        /// <summary>
        /// 当前页面列表
        /// </summary>
        public IList<T> PageItems { get; set; }

        public IEnumerator<T> GetEnumerator()
        {
            return PageItems.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return PageItems.GetEnumerator();
        }
    }

}
