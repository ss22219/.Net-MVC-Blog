using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.WebUI.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "请填写用户名。")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入一个密码。")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "您的密码长度最少需要6个字符。")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "两次密码不一致。")]
        public string RePassword { get; set; }

        [RegularExpression(@"^\w+@\w+\.\w+$", ErrorMessage = "电子邮件地址不正确。")]
        public string Email { get; set; }

    }
}