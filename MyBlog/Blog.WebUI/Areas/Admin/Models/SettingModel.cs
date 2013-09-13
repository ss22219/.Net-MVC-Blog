using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class SettingModel
    {
        [Required(ErrorMessage = "博客标题不能为空。")]
        public string BlogTitle { get; set; }

        public string BlogDescription { get; set; }
        [Range(0, 6, ErrorMessage = "星期开始设置不正确。")]
        public int WeekStart { get; set; }

        public bool AllRegister { get; set; }
        [Range(0, 2, ErrorMessage = "默认角色错误。")]
        public int DefaultRole { get; set; }

        public string WebSite { get; set; }

        [Range(1, 999999, ErrorMessage = "对不起，最新文章数错误。")]
        public int LastArticleCount { get; set; }
        [Range(1, 999999, ErrorMessage = "对不起，分类标签页文章数错误。")]
        public int ArticlePageSize { get; set; }

        public bool NoLoginComment { get; set; }

        public bool CommentSatus { get; set; }


        [Range(1, 999999, ErrorMessage = "对不起，评论分页数错误。")]
        public int CommentPageSize { get; set; }


        [Range(0, 999999, ErrorMessage = "对不起，评论镶套数错误。")]
        public int MaxReComment { get; set; }
    }
}