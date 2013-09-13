using Blog.Domain;
using Blog.Domain.Interface;
using Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.WebUI.Areas.Admin.Models
{
    public static class CategoryHelper
    {
        public static MvcHtmlString BulidCategoryList(this HtmlHelper Html, IList<Category> list = null, int categoryId = 0)
        {
            if (list == null)
            {
                list = DependencyResolver.Current.GetService<ICategoryService>().GetAllCategory().Where(c => c.Type == Domain.CategoryType.Category && c.Parent == null).ToList();
            }
            string str = "";
            int level = 0;
            str += BulidChildrenCategory(list, level, categoryId);
            return MvcHtmlString.Create(str);
        }

        private static string BulidChildrenCategory(IList<Category> categorys, int level, int categoryId = 0)
        {
            string str = "";
            foreach (var cat in categorys)
            {
                TagBuilder option = new TagBuilder("option");
                option.Attributes.Add("value", cat.CategoryId.ToString());
                option.AddCssClass("level-" + level);
                string span = "";
                for (int i = 0; i < (level * 3); i++)
                {
                    span += "&nbsp;";
                }
                option.InnerHtml = span + cat.Name;
                if (categoryId == cat.CategoryId)
                {
                    option.Attributes.Add("selected", "selected");
                }
                str += option.ToString();
                if (cat.Children != null && cat.Children.Count > 0)
                {
                    str += BulidChildrenCategory(cat.Children, level + 1, categoryId);
                }
            }
            return str;
        }

        public static MvcHtmlString GetCategoryString(this HtmlHelper Html, Article articel)
        {
            string str = "";
            var list = articel.Categorys.Where(c => c.Type == CategoryType.Category).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (i < list.Count - 1)
                    str += list[i].Name + ",";
                else
                    str += list[i].Name;
            }
            return MvcHtmlString.Create(str);
        }

        public static MvcHtmlString GetTagString(this HtmlHelper Html, Article articel)
        {
            string str = "";
            var list = articel.Categorys.Where(c => c.Type == CategoryType.Tag).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (i < list.Count - 1)
                    str += list[i].Name + ",";
                else
                    str += list[i].Name;
            }
            if (string.IsNullOrEmpty(str))
            {
                str = "-";
            }
            return MvcHtmlString.Create(str);
        }

        /// <summary>
        /// 创建文章的分类选择
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        public static MvcHtmlString BulidArticleCategory(this HtmlHelper Html, Article article)
        {
            var list = DependencyResolver.Current.GetService<ICategoryService>().GetAllCategory().Where(c => c.Type == Domain.CategoryType.Category && c.Parent == null).ToList();
            int level = 0;
            TagBuilder ul = new TagBuilder("ul");
            ul.Attributes.Add("class", "categorychecklist form-no-clear");
            ul.Attributes.Add("id", "categorychecklist");
            ul.Attributes.Add("data-wp-lists", "list:category");
            foreach (var cat in list)
            {
                TagBuilder li = new TagBuilder("li");
                li.Attributes.Add("id", "category-" + cat.CategoryId);
                TagBuilder label = new TagBuilder("label");
                label.AddCssClass("selectit");
                TagBuilder input = new TagBuilder("input");
                input.Attributes.Add("value", cat.CategoryId.ToString());
                input.Attributes.Add("type", "checkbox");
                input.Attributes.Add("name", "Category");
                input.Attributes.Add("id", "in-category-" + cat.CategoryId);
                if (article.Categorys != null && article.Categorys.Count > 0 && article.Categorys.Where(c => c.CategoryId == cat.CategoryId).Count() > 0)
                {
                    input.Attributes.Add("checked", "checked");
                }
                label.InnerHtml = input.ToString() + cat.Name;
                li.InnerHtml = label.ToString();
                if (cat.Children != null && cat.Children.Count > 0)
                {
                    li.InnerHtml += BulidArticleChildrenCategory(cat.Children, level + 1, article);
                }
                ul.InnerHtml += li.ToString();
            }
            return MvcHtmlString.Create(ul.ToString());
        }

        private static string BulidArticleChildrenCategory(IList<Category> categorys, int level, Article article)
        {
            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("children");
            foreach (var cat in categorys)
            {
                TagBuilder li = new TagBuilder("li");
                li.Attributes.Add("id", "category-" + cat.CategoryId);
                TagBuilder label = new TagBuilder("label");
                label.AddCssClass("selectit");
                TagBuilder input = new TagBuilder("input");
                input.Attributes.Add("value", cat.CategoryId.ToString());
                input.Attributes.Add("type", "checkbox");
                input.Attributes.Add("name", "Category");
                input.Attributes.Add("id", "in-category-" + cat.CategoryId);
                if (article.Categorys != null && article.Categorys.Count > 0 && article.Categorys.Where(c => c.CategoryId == cat.CategoryId).Count() > 0)
                {
                    input.Attributes.Add("checked", "checked");
                }
                label.InnerHtml = input.ToString() + cat.Name;
                li.InnerHtml = label.ToString();
                if (cat.Children != null && cat.Children.Count > 0)
                {
                    li.InnerHtml += BulidArticleChildrenCategory(cat.Children, level + 1, article);
                }
                ul.InnerHtml += li.ToString();
            }
            return ul.ToString();
        }
    }
}