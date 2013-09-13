using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "分类名称不能为空！")]
        public string Name { get; set; }

        public int ParentId { get; set; }
    }
}