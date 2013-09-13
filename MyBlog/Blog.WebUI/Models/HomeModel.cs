using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Domain.Interface;
using Blog.Model;
namespace Blog.WebUI.Models
{
    public class HomeModel
    {
        public IList<Category> Categorys { get; set; }

        public IList<Article> LastBlogs { get; set; }

        public IDictionary<string, string> DateCategory { get; set; }

        public IList<Article> LastPage { get; set; }
    }
}