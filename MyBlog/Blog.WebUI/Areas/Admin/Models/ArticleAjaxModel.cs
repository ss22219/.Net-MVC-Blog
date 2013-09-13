using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class ArticleAjaxModel
    {
        public int ArticleId { get; set; }

        [Required(ErrorMessage = "文章标题不能为空！")]
        [StringLength(50, ErrorMessage = "文章标题长度不能超过50个字符")]
        public string Title { get; set; }

        public string CreateDate { get; set; }

        [Required(ErrorMessage = "文章分类不能为空！")]
        public int[] Categorys { get; set; }

        public string[] Tags { get; set; }

        public int? Status { get; set; }

        [Required(ErrorMessage = "文章内容不能为空！")]
        public string Content { get; set; }
    }
}