using Blog.Domain;
using Blog.Domain.Interface;
using Blog.Model;
using Blog.WebUI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.WebUI.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;
        private ISettingService _settingService;

        public UserController(ISettingService settingService, IUserService userService)
        {
            _userService = userService;
            _settingService = settingService;
        }

        [AdminAuthorize]
        public ActionResult Index(UserSearchModel model, string message = null, string method = null, int[] userIds = null)
        {
            ///操作判断开始
            if (!string.IsNullOrEmpty(method))
            {
                if (method.Equals("Delete"))
                {
                    return DeleteList(userIds);
                }
            }
            ViewBag.Message = message;
            ///取得每页显示数            
            int pageSize = int.Parse(_settingService.GetSetting("AdminUserPageSize"));
            if (model.PageSize != null && model.PageSize > 0 && model.PageSize != pageSize)
            {
                _settingService.SetSetting("AdminUserPageSize", model.PageSize.ToString());
            }
            else
            {
                model.PageSize = pageSize;
            }
            PageInfo<User> pageInfo = new PageInfo<User>();
            int count = (int)_userService.Single(query => BulidQuery(query, model).Count());


            model.AllCount = (int)_userService.Single(query => query.Count());
            model.AdminCount = (int)_userService.Single(query => query.Where(u => u.Role == UserRole.Admin).Count());
            model.ReaderCount = (int)_userService.Single(query => query.Where(u => u.Role == UserRole.Reader).Count());

            pageInfo.TotalItem = count;
            pageInfo.PageSize = model.PageSize.Value;

            model.PageIndex = model.PageIndex > pageInfo.TotalPage ? pageInfo.TotalPage : model.PageIndex;

            IList<User> list = _userService.Find(query => BulidQuery(query, model).OrderByDescending(c => c.RegisterDate).Skip((model.PageIndex - 1) * model.PageSize.Value).Take(model.PageSize.Value));
            pageInfo.PageItems = list;
            model.PageInfo = pageInfo;
            return View(model);
        }


        public IQueryable<User> BulidQuery(IQueryable<User> query, UserSearchModel model)
        {
            if (model.Role != null)
                query = query.Where(u => u.Role == model.Role);
            if (!string.IsNullOrEmpty(model.Search))
                query = query.Where(u => u.UserName.Contains(model.Search) || u.NiceName.Contains(model.Search));
            return query;
        }

        public ActionResult Delete(int id)
        {
            string message = "1 个用户成功删除。";
            try
            {
                _userService.Delete(id);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return RedirectToAction("Index", new { message = message });
        }

        public ActionResult DeleteList(int[] userIds)
        {
            string message = userIds.Length + " 个用户成功删除。";
            try
            {
                foreach (int id in userIds)
                    _userService.Delete(id);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return RedirectToAction("Index", new { message = message });
        }

        [UserAuthorize]
        [HttpGet]
        public ActionResult Info()
        {
            return View(_userService.GetLoginUser());
        }

        [UserAuthorize]
        [HttpPost]
        public ActionResult Info(UserInfoModel model)
        {
            User user = _userService.GetLoginUser();
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        user.Password = model.Password;
                    }
                    user.NiceName = model.NiceName;


                    if (Request.Files.Count > 0 && !string.IsNullOrEmpty(model.Picture))
                    {
                        string extension = Path.GetExtension(Request.Files[0].FileName).ToLower();

                        if (extension == ".jpg" || extension == ".png" || extension == ".bmp")
                        {

                            if (user.Extends.ContainsKey("Picture"))
                            {
                                if (System.IO.File.Exists(Server.MapPath("~/") + user.Extends["Picture"]))
                                {
                                    System.IO.File.Delete(Server.MapPath("~/") + user.Extends["Picture"]);
                                }
                            }
                            string path = "/Content/Upload/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
                            string savePath = Server.MapPath("~/") + path;

                            if (!Directory.Exists(savePath))
                            {
                                Directory.CreateDirectory(savePath);
                            }
                            Random r = new Random();
                            int length = r.Next(10);
                            string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + GetRandomString(length) + extension;
                            Request.Files[0].SaveAs(savePath + fileName);
                            user.Extends["Picture"] = path + fileName;
                        }
                        else
                        {
                            throw new Exception("对不起，不支持该格式的头像。目前只支持 .jpg、.png、.bmp格式的头像");
                        }
                    }
                    _userService.UpdateUser(user);
                }
                catch (Domain.DomainException ex)
                {
                    ViewBag.Message = ex.Errors.First().Value;
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }
            }
            else
            {
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        ViewBag.Message = error.ErrorMessage;
                        break;
                    }
                    if (ViewBag.Message != null)
                    {
                        break;
                    }
                }
            }

            return View(user);
        }

        [AdminAuthorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                User user = _userService.GetUserById(id);

                return View("Info", user);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { message = ex.Message });
            }
        }

        [AdminAuthorize]
        [HttpPost]
        public ActionResult Edit(UserInfoModel model, int id)
        {
            User user = null;
            try
            {
                user = _userService.GetUserById(id);
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(model.Password))
                        {
                            user.Password = model.Password;
                        }
                        user.NiceName = model.NiceName;


                        if (Request.Files.Count > 0 && !string.IsNullOrEmpty(model.Picture))
                        {
                            string extension = Path.GetExtension(Request.Files[0].FileName).ToLower();

                            if (extension == ".jpg" || extension == ".png" || extension == ".bmp")
                            {

                                if (user.Extends.ContainsKey("Picture"))
                                {
                                    if (System.IO.File.Exists(Server.MapPath("~/") + user.Extends["Picture"]))
                                    {
                                        System.IO.File.Delete(Server.MapPath("~/") + user.Extends["Picture"]);
                                    }
                                }
                                string path = "/Content/Upload/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
                                string savePath = Server.MapPath("~/") + path;

                                if (!Directory.Exists(savePath))
                                {
                                    Directory.CreateDirectory(savePath);
                                }
                                Random r = new Random();
                                int length = r.Next(10);
                                string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + GetRandomString(length) + extension;
                                Request.Files[0].SaveAs(savePath + fileName);
                                user.Extends["Picture"] = path + fileName;
                            }
                            else
                            {
                                throw new Exception("对不起，不支持该格式的头像。目前只支持 .jpg、.png、.bmp格式的头像");
                            }
                        }
                        _userService.UpdateUser(user);
                        return RedirectToAction("Index", new { message = "编辑成功。" });
                    }
                    catch (Domain.DomainException ex)
                    {
                        ViewBag.Message = ex.Errors.First().Value;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = ex.Message;
                    }
                }
                else
                {
                    foreach (var value in ModelState.Values)
                    {
                        foreach (var error in value.Errors)
                        {
                            ViewBag.Message = error.ErrorMessage;
                            break;
                        }
                        if (ViewBag.Message != null)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            if (user != null)
            {
                return View("Info", user);
            }
            else
            {
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
        }

        [AdminAuthorize]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [AdminAuthorize]
        [HttpPost]
        public ActionResult Add(UserAddModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = new User() { UserName = model.UserName, Email = model.Email, Role = model.Role, RegisterDate = DateTime.Now, NiceName = model.UserName };
                    _userService.AddUser(user);
                    return RedirectToAction("Index", new { Message = "用户添加成功。" });
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }
            }
            else
            {

                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        ViewBag.Message = error.ErrorMessage;
                        break;
                    }
                    if (ViewBag.Message != null)
                    {
                        break;
                    }
                }
            }
            return View();
        }


        private string GetRandomString(int length)
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                str += (char)random.Next(65, 112);
            }
            str = str.Replace(@"\", "_");
            str = str.Replace("/", "_");
            return str;
        }
    }

}
