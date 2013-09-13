using Blog.Domain;
using Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class UserSearchModel
    {
        public string Search { get; set; }

        public int? Role { get; set; }

        public int PageIndex { get; set; }

        public int? PageSize { get; set; }

        public int? AllCount { get; set; }

        public int? ReaderCount { get; set; }

        public int? AdminCount { get; set; }

        public PageInfo<User> PageInfo { get; set; }

        public UserSearchModel Build(int pageIndex = 0)
        {
            UserSearchModel model = new UserSearchModel() { Search = Search, Role = Role, PageIndex = PageIndex, PageSize = PageSize };
            if (pageIndex != 0)
            {
                model.PageIndex = pageIndex;
            }
            return model;
        }
    }
}