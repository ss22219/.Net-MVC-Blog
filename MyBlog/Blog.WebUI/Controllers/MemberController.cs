using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Model;
using Blog.WebUI.Models;
using Blog.Domain.Interface;
using Blog.Domain;

namespace Blog.WebUI.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    public class MemberController : Controller
    {
        /// <summary>
        /// 由容器注入
        /// </summary>
        private IUserService _userService;

        public MemberController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 用户首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (_userService.GetLoginUser() == null)
                return View("Login");
            else
                return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="logout"></param>
        /// <returns></returns>
        public ActionResult Login(string ReturnUrl = "")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View("Login");
        }

        /// <summary>
        /// 登陆处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            User user = null;
            if (ModelState.IsValid)
            {
                try
                {
                    user = _userService.Login(model.UserName, model.Password);
                }
                catch (Domain.DomainException ex)
                {
                    foreach (string key in ex.Errors.Keys)
                    {
                        ModelState.AddModelError(key, ex.Errors[key]);
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = new List<string>();
                foreach (ModelState state in ModelState.Values)
                {
                    foreach (ModelError error in state.Errors)
                    {
                        ViewBag.Errors.Add(error.ErrorMessage);
                    }
                }
            }

            if (user != null)
            {
                if (model.RememberMe == true)
                {
                    Response.SetCookie(new HttpCookie("UserName", model.UserName));
                    Response.SetCookie(new HttpCookie("Password", model.Password));
                }
                else
                {
                    Response.SetCookie(new HttpCookie("UserName", string.Empty));
                    Response.SetCookie(new HttpCookie("Password", string.Empty));
                }
                System.Web.Security.FormsAuthentication.SetAuthCookie(user.UserName, false);
                if (string.IsNullOrEmpty(model.ReturnUrl))
                {
                    if (user.Role == UserRole.Admin)
                        return RedirectToAction("Index", "Main", new { area = "Admin" });
                    else
                        return RedirectToAction("Info", "User", new { area = "Admin" });
                }
                else
                    return Redirect(model.ReturnUrl);

            }
            else
            {
                return View("Login");
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            _userService.LogOut();
            ViewBag.Message = "您已经成功登出。";
            return Login();
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <returns></returns>
        public ActionResult LostPassword()
        {
            return View();
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LostPassword(string UserName)
        {
            if (string.IsNullOrEmpty(UserName.Trim()))
            {
                ModelState.AddModelError("UserName", "请输入用户名或电子邮件地址。");
            }
            else
            {
                try
                {
                    _userService.SendNewPassword(UserName);
                    if (ModelState.IsValid)
                    {
                        ViewBag.Message = "新密码已经发送到您的邮箱。";
                        return Login();
                    }
                }
                catch (Domain.DomainException ex)
                {
                    foreach (string key in ex.Errors.Keys)
                    {
                        ModelState.AddModelError(key, ex.Errors[key]);
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = new List<string>();
                foreach (ModelState state in ModelState.Values)
                {
                    foreach (ModelError error in state.Errors)
                    {
                        ViewBag.Errors.Add(error.ErrorMessage);
                    }
                }
            }

            return View();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            if (ViewBag.AllRegister == "0")
            {
                ViewBag.Errors = new List<string>() { "本站已经关闭注册功能。" };
                return Login();
            }
            ///设置正在注册
            ViewBag.IsRegisting = true;
            return View();
        }

        /// <summary>
        /// 注册处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _userService.Register(new User() { UserName = model.UserName, Password = model.Password, Email = model.Email });
                }
                catch (Domain.DomainException ex)
                {
                    foreach (string key in ex.Errors.Keys)
                    {
                        ModelState.AddModelError(key, ex.Errors[key]);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("DataBaseError", ex.Message);
                }
                if (ModelState.IsValid)
                {
                    ViewBag.Message = "注册成功。";
                    return Login();
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = new List<string>();
                foreach (ModelState state in ModelState.Values)
                {
                    foreach (ModelError error in state.Errors)
                    {
                        ViewBag.Errors.Add(error.ErrorMessage);
                    }
                }
            }
            return View();
        }
    }
}
