using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.WebUI.Models
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 最大页数
        /// </summary>
        public int TotalPage { get; set; }
        /// <summary>
        /// 显示页数
        /// </summary>
        public int ShowPage { get; set; }
        /// <summary>
        /// 访问的Action方法
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string Cotroller { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }
    }
}