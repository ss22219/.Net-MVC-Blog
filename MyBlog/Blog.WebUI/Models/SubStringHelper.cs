using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;

namespace Blog.WebUI.Models
{
    /// <summary>
    /// 嘿嘿，这只是一个分本截断类
    /// </summary>
    public static class SubStringHelper
    {
        public static MvcHtmlString SubString(this HtmlHelper Html, string str)
        {
            if (str.Length > 50)
            {
                str = str.Substring(0, 50) + "...";
            }
            return MvcHtmlString.Create(str);
        }

        public static MvcHtmlString SubString(this HtmlHelper Html, string str, int length, string substring)
        {
            if (str.Length > length)
            {
                str = str.Substring(0, length) + substring;
            }
            return MvcHtmlString.Create(str);
        }
    }
}