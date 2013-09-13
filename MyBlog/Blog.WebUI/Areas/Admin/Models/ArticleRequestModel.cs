using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class ArticleRequestModel
    {
        public int? Category { get; set; }

        public int PageSize { get; set; }

        public bool ExcerptMode { get; set; }

        public ArticleOrder? OrderBy { get; set; }

        public string MonthCategory { get; set; }

        public OrderType? OrderType { get; set; }

        public int Page { get; set; }

        public string Title { get; set; }

        public string Tag { get; set; }

        public int? Status { get; set; }
    }
}
