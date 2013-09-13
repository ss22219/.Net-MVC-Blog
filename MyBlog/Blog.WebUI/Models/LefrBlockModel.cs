using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Model;
namespace Blog.WebUI.Models
{
    /// <summary>
    /// 左边样式模型
    /// </summary>
    public class SidebarModel
    {
        /// <summary>
        /// 最新文章 
        /// </summary>
        public IList<Article> LastBlog { get; set; }

        /// <summary>
        /// 最新评论
        /// </summary>
        public IList<Comment> LastComment { get; set; }

        /// <summary>
        /// 月份分类归档
        /// </summary>
        public IDictionary<string, string> MonthCategory { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public IList<Category> Categorys { get; set; }
    }
}