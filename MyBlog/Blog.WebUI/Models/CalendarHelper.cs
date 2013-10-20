using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Blog.Model;
using Blog.Domain.Interface;
using System.Web.Caching;

namespace Blog.WebUI.Models
{
    /// <summary>
    /// 日期辅助类
    /// </summary>
    public static class CalendarHelper
    {
        /// <summary>
        /// 生成日历，基于面向过程
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="weekStart">一周从哪一天开始，0是周末</param>
        /// <returns>返回Html</returns>

        public static MvcHtmlString Calendar(this HtmlHelper Html, int weekStart)
        {
            MvcHtmlString str = null;
            ///中国一周中的天
            IList<string> cDays = new List<string>(){
                "日",
                "一",
                "二",
                "三",
                "四",
                "五",
                "六"
            };

            StringBuilder sb = new StringBuilder();

            ///构造表头
            sb.AppendFormat("<table id=\"wp-calendar\"><caption>{0}</caption><thead><tr>", DateTime.Now.ToString("yyyy 年MM月"));

            ///一个月中第几天
            int monthDay = 0;
            ///一周中的第几天
            int weekDay = 0;

            ///构造星期表头
            for (monthDay = 0; monthDay < 7; monthDay++)
            {
                weekDay = monthDay + weekStart > 6 ? monthDay + weekStart - 7 : monthDay + weekStart;
                sb.AppendFormat("<th scope=\"col\" title=\"星期{0}\">{0}</th>", cDays[weekDay]);
            }
            sb.Append("</tr></thead>");
            sb.Append("<tbody><tr>");

            ///一个月的开始日期
            DateTime StartDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ///一个月的结束日期
            DateTime EndDay = StartDay.AddMonths(1).AddDays(-1);

            ///本月文章列表
            IList<Article> articles = DependencyResolver.Current.GetService<IArticleService>().FindArticleByMonth(StartDay, 1).PageItems;

            monthDay = 0;
            weekDay = 0;

            ///构造空表格
            while (true)
            {
                if ((int)StartDay.DayOfWeek == (weekStart + monthDay > 6 ? monthDay + weekStart - 7 : monthDay + weekStart))
                {
                    break;
                }
                sb.Append("<td></td>");
                monthDay++;
                weekDay++;
            }

            ///构造日期表格
            for (monthDay = 0; monthDay < EndDay.Day; monthDay++)
            {
                ///当天文章
                string data = "";
                var query = articles.Where(a => a.CreateDate.Day == monthDay + StartDay.Day);
                if (query.Count() > 0)
                {
                    string title = "";
                    foreach (Article item in query)
                    {
                        title += item.Title + " ";
                    }
                    DateTime now = StartDay.AddDays(monthDay);
                    data = "<a href=\"/Category/Day/" + now.ToString("yyyyMMdd") + "\" title=\"" + title + "\" >" + now.Day + "</a>";

                }
                else
                {
                    data = (StartDay.Day + monthDay).ToString();
                }


                ///今天高亮
                if (monthDay + StartDay.Day == DateTime.Now.Day)
                    sb.AppendFormat("<td id=\"today\">{0}</td>", data);
                else
                    sb.AppendFormat("<td>{0}</td>", data);
                weekDay++;


                ///一周换横
                if (weekDay > 6)
                {
                    sb.Append("</tr><tr>");
                    weekDay = 0;
                }
            }

            sb.Append("</tr></tbody></table>");

            str = MvcHtmlString.Create(sb.ToString());
            return str;
        }
    }
}