using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class UserInfoModel
    {
        [Required(ErrorMessage = "昵称不能为空。")]
        public string NiceName { get; set; }

        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "两次密码不一致。")]
        public string RePassword { get; set; }

        public string Picture { get; set; }
    }
}