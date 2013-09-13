using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Blog.WebUI.Models
{
    /// <summary>
    /// 评论模型
    /// </summary>
    public class CommentModel
    {
        [Required(ErrorMessage = "对不起，您的来源不正确。")]
        public int ArticleId { get; set; }

        public int ParentId { get; set; }

        [StringLength(20, ErrorMessage = "昵称不能超过20个字符长度。")]
        public string Author { get; set; }

        [RegularExpression(@"^\w+@\w+\.\w+$", ErrorMessage = "电子邮件地址不正确。")]
        [StringLength(20, ErrorMessage = "邮箱不能超过20个字符长度。")]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "网站不能超过50个字符长度。")]
        public string Url { get; set; }

        [Required(ErrorMessage = "对不起，评论内容不能为空。")]
        [StringLength(1000, ErrorMessage = "评论内容不能超过1千个字符长度。")]
        public string Content { get; set; }

    }
}