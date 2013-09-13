using System;
using Blog.Domain;
using Blog.Domain.Interface;
using Blog.Model;
using Blog.WebUI.Areas.Admin.Models;
using Blog.WebUI.Areas.Admin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.WebUI.Areas.Admin.Controllers
{
    public class SettingController : Controller
    {
        private ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [AdminAuthorize]
        [HttpGet]
        public ActionResult Index()
        {
            return View(_settingService.GetAllSetting());
        }


        [AdminAuthorize]
        [HttpPost]
        public ActionResult Index(SettingModel model)
        {
            if (ModelState.IsValid)
            {
                _settingService.SetSetting("AllRegister", model.AllRegister ? "1" : "0");
                _settingService.SetSetting("BlogDescription", model.BlogDescription);
                _settingService.SetSetting("BlogTitle", model.BlogTitle);
                _settingService.SetSetting("DefaultRole", model.DefaultRole.ToString());
                _settingService.SetSetting("WeekStart", model.WeekStart.ToString());
                _settingService.SetSetting("WebSite", model.WebSite.ToString());

                _settingService.SetSetting("LastArticleCount", model.LastArticleCount.ToString());
                _settingService.SetSetting("ArticlePageSize", model.ArticlePageSize.ToString());

                _settingService.SetSetting("CommentPageSize", model.CommentPageSize.ToString());
                _settingService.SetSetting("MaxReComment", model.MaxReComment.ToString());
                _settingService.SetSetting("NoLoginComment", model.NoLoginComment ? "0" : "1");
                _settingService.SetSetting("CommentSatus", model.CommentSatus ? "0" : "1");

                ViewBag.Message = "设置更新成功。";
            }
            else
            {
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        ViewBag.Error = error.ErrorMessage;
                        break;
                    }
                    if (ViewBag.Error != null)
                    {
                        break;
                    }
                }
            }

            return View(_settingService.GetAllSetting());
        }
    }
}
