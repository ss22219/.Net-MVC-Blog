using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Domain;
using System.Web.Mvc;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class UserAddModel
    {
        [Required(ErrorMessage = "用户名不能为空。")]
        [StringLength(20, ErrorMessage = "用户名不能超过20个字符。")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空。")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度不能少于6位数，大于20。")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "两次密码不一致。")]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "邮箱地址不能为空。")]
        [RegularExpression(@"^\w+@\w+\.\w+$", ErrorMessage = "邮箱格式不正确。")]
        public string Email { get; set; }

        [Range(1, 2, ErrorMessage = "用户角色不正确。")]
        public int Role { get; set; }
    }
}