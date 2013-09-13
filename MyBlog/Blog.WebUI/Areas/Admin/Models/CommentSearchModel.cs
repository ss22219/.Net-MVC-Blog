using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Domain;
using Blog.Model;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class CommentSearchModel
    {
        private int pageIndex = 1;

        public int? Status { get; set; }

        public string Search { get; set; }

        public int PageIndex { get { return pageIndex; } set { if (pageIndex > 0) { pageIndex = value; } } }

        public int? PageSize { get; set; }

        public string IpAddress { get; set; }

        public PageInfo<Comment> PageInfo { get; set; }

        public int? AllCount { get; set; }

        public int? OpenCount { get; set; }

        public int? VerifyCount { get; set; }

        public int? DeleteCount { get; set; }

        public CommentSearchModel Build(int pageIndex = 0)
        {
            CommentSearchModel model = new CommentSearchModel()
            {
                pageIndex = this.pageIndex,
                Status = Status,
                Search = Search,
                PageSize = PageSize,
                IpAddress = IpAddress
            };
            if (pageIndex != 0)
            {
                model.pageIndex = pageIndex;
            }
            return model;
        }
    }
}