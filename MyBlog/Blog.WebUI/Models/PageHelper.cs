using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
namespace Blog.WebUI.Models
{
    /// <summary>
    /// 分页辅助类
    /// </summary>
    public static class PageHelper
    {
        /// <summary>
        /// 分页辅助
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static MvcHtmlString Page(this HtmlHelper Html, UrlHelper Url, PageInfo info)
        {
            StringBuilder builder = new StringBuilder();
            if (info.TotalPage == 1)
                return MvcHtmlString.Empty;
            int pageStart = (int)Math.Floor((double)info.PageIndex / (double)(info.ShowPage - 1));
            pageStart = pageStart == 0 ? 1 : pageStart;
            for (int i = pageStart; i < info.ShowPage + pageStart && i <= info.TotalPage; i++)
            {
                TagBuilder a = new TagBuilder("a") { InnerHtml = i.ToString() };
                if (i == info.PageIndex)
                {
                    a.AddCssClass("current");
                }
                a.Attributes.Add("href", Url.Action(info.Action, info.Cotroller, new { page = i, area = info.Area }, null));
                builder.Append(a.ToString());
            }
            return MvcHtmlString.Create(builder.ToString());
        }
    }
}