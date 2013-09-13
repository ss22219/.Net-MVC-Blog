using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Blog.Model;
using Blog.Domain.Interface;
using Blog.Domain;

namespace Blog.WebUI.Models
{
    /// <summary>
    /// 视图辅助类
    /// </summary>
    public static class BlogHelper
    {
        /// <summary>
        /// Html到文本
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string Html2String(this HtmlHelper Html, string html)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[/s/S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[/s/S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" no[/s/S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[/s/S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[/s/S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@"/<img[^/>]+/>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex7 = new System.Text.RegularExpressions.Regex(@"</p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex8 = new System.Text.RegularExpressions.Regex(@"<p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex9 = new System.Text.RegularExpressions.Regex(@"<[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记
            html = regex1.Replace(html, ""); //过滤<script></script>标记
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件
            html = regex4.Replace(html, ""); //过滤iframe
            html = regex5.Replace(html, ""); //过滤frameset
            html = regex6.Replace(html, ""); //过滤frameset
            html = regex7.Replace(html, ""); //过滤frameset
            html = regex8.Replace(html, ""); //过滤frameset
            html = regex9.Replace(html, "");
            html = html.Replace("&nbsp;", " ");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            return html;
        }

        /// <summary>
        /// 取得用户的头像
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Url"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string Picture(this HtmlHelper Html, UrlHelper Url, User user)
        {
            string picture = Url.Content("~/Content/Include/images/picture.png");
            if (user != null && user.Extends.ContainsKey("Picture"))
            {
                picture = user.Extends["Picture"];
            }
            return picture;
        }

        /// <summary>
        /// 对内容进行编码输出
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Encode(this HtmlHelper Html, string content)
        {
            return Html.Encode(content).Replace("\n", "<br/>");
        }

        /// <summary>
        /// 递归生成评论列表，基于面向对象（可以不用这个）
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="comments"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static MvcHtmlString CommentList(this HtmlHelper Html, UrlHelper Url, IList<Comment> comments)
        {
            ICommentService commentService = DependencyResolver.Current.GetService<ICommentService>();
            StringBuilder sb = new StringBuilder();
            TagBuilder ol = new TagBuilder("ol");
            ol.AddCssClass("commentlist");

            foreach (var comment in comments)
            {
                sb.AppendLine(BulidComment(commentService, Html, Url, comment));

            }
            ol.InnerHtml = sb.ToString();

            ///垃圾回收
            GC.Collect();
            return MvcHtmlString.Create(ol.ToString());
        }

        /// <summary>
        /// 生成子评论
        /// </summary>
        /// <param name="commentService"></param>
        /// <param name="Html"></param>
        /// <param name="Url"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        private static string BulidChildren(ICommentService commentService, HtmlHelper Html, UrlHelper Url, IList<Comment> comments)
        {

            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("children");
            foreach (var comment in comments)
            {
                ul.InnerHtml += BulidComment(commentService, Html, Url, comment);
            }
            return ul.ToString();
        }

        /// <summary>
        /// 生成单个评论
        /// </summary>
        private static string BulidComment(ICommentService commentService, HtmlHelper Html, UrlHelper Url, Comment comment)
        {
            TagBuilder commentTag = new TagBuilder("li");
            commentTag.Attributes.Add("class", "comment byuser comment-author-admin bypostauthor odd alt depth-3");
            commentTag.Attributes.Add("id", "comment-" + comment.CommentId);

            TagBuilder div = new TagBuilder("div");
            div.Attributes.Add("id", "div-comment-" + comment.CommentId);
            div.AddCssClass("comment-body");

            TagBuilder authorDiv = new TagBuilder("div");
            authorDiv.Attributes.Add("class", "comment-author vcard");

            TagBuilder img = new TagBuilder("img");
            img.Attributes.Add("src", Picture(Html, Url, comment.User));
            img.Attributes.Add("class", "avatar avatar-32 photo");
            img.Attributes.Add("height", "32");
            img.Attributes.Add("width", "32");

            TagBuilder cite = new TagBuilder("cite");
            cite.AddCssClass("fn");
            cite.InnerHtml = comment.User == null ? comment.Author : comment.User.NiceName;
            TagBuilder span = new TagBuilder("span");
            span.AddCssClass("says");
            span.InnerHtml = " 说道：";

            if (comment.Status == CommentStatus.Verify)
            {
                span.InnerHtml += "&nbsp;&nbsp;(等待审核中)";
            }
            authorDiv.InnerHtml = img.ToString() + cite.ToString() + span.ToString();

            TagBuilder metaDiv = new TagBuilder("div");
            metaDiv.AddCssClass("comment-meta commentmetadata");
            metaDiv.InnerHtml = new TagBuilder("a") { InnerHtml = comment.CreateDate.ToString("yyyy 年 M 月 d 日 " + (comment.CreateDate.Hour <= 12 ? "上午" : "下午") + " h:m") }.ToString();
            if (DependencyResolver.Current.GetService<IRoleService>().AdminPower())
            {
                metaDiv.InnerHtml += "<a class=\"comment-edit-link\" href=\"" + Url.Action("Edit", "Comment", new { area = "Admin", id = comment.CommentId }) + "\" title=\"编辑评论\">(编辑)</a>";
            }
            TagBuilder content = new TagBuilder("p") { InnerHtml = Encode(Html, comment.Content) };

            div.InnerHtml = authorDiv.ToString() + metaDiv.ToString() + content.ToString();
            if (comment.Status == CommentStatus.Open)
            {
                TagBuilder replyDiv = new TagBuilder("div");
                replyDiv.AddCssClass("reply");
                TagBuilder replyLink = new TagBuilder("a") { InnerHtml = "回复" };
                replyLink.AddCssClass("comment-reply-link");
                replyLink.Attributes.Add("src", "reComment=" + comment.CommentId + "#respond");
                replyLink.Attributes.Add("onclick", "return addComment.moveForm('div-comment-" + comment.CommentId + "', '" + comment.CommentId + "', 'respond', '" + comment.Article.ArticleId + "')");

                replyDiv.InnerHtml = replyLink.ToString();
                div.InnerHtml += replyDiv.ToString();
            }
            commentTag.InnerHtml = div.ToString();

            IList<Comment> childrenComment = commentService.GetCommentsByParentId(comment.CommentId);
            if (childrenComment.Count > 0)
            {
                commentTag.InnerHtml += (BulidChildren(commentService, Html, Url, childrenComment));
            }
            return commentTag.ToString();
        }
    }
}