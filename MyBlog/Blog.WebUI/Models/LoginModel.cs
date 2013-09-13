using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.WebUI.Models
{
    /// <summary>
    /// 登陆模型 （该类型为视图模型，User为领域模型）
    /// </summary>
    public class LoginModel
    {
        [Required(ErrorMessage = "用户名不能为空！")]
        [StringLength(20, ErrorMessage = "密码长度不能超过20个字符")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空！")]
        [StringLength(20, ErrorMessage = "密码长度不能超过20个字符")]
        [Display(Name = "pwd")]
        public string Password { get; set; }

        public bool? RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}