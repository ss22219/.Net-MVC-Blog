using System;
using Blog.Domain.Interface;
using Blog.Repository.Interface;
using Blog.Model;

namespace Blog.Domain
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IRoleService _roleService;
        private ISettingService _settingService;
        private ISessionManager _sessionManager;

        public UserService(IUserRepository userRepository, ISessionManager sesstionService, IRoleService roleService, ISettingService settingService, ISessionManager sessionManager)
        {
            _userRepository = userRepository;
            _roleService = roleService;
            _sessionManager = sessionManager;
            _settingService = settingService;
        }
        public User Login(string userName, string Password)
        {
            User user = _userRepository.CheckUser(userName, Password);
            if (user != null)
            {
                _sessionManager.User = user;
                return user;
            }
            else
            {
                throw new DomainException("UserName", "用户名或密码不正确！");
            }
        }

        public void RePassword(Model.User user, string oldPasswrod, string newPassword)
        {
            throw new NotImplementedException();
        }


        public void SendNewPassword(string UserName)
        {
            if (_userRepository.CheckUserNameExist(UserName) || _userRepository.CheckEmailExist(UserName))
            {
                throw new DomainException("UserName", "真抱歉，我们的Mail服务器崩溃了！<br/>为此，你可以寻找管理员帮忙为您解决问题。");
            }
            else
            {
                throw new DomainException("UserName", "用户名或电子邮件地址无效。");
            }
        }


        public void Register(User user)
        {
            if (!_roleService.RegisterPower())
            {
                throw new DomainException("Error", "本站已经关闭注册功能。");
            }
            else if (_userRepository.CheckUserNameExist(user.UserName))
            {
                throw new DomainException("UserName", "该用户名已经被注册。");
            }
            else if (_userRepository.CheckEmailExist(user.Email))
            {
                throw new DomainException("Email", "该邮箱已经被注册。");
            }
            else
            {
                user.NiceName = user.UserName;

                user.RegisterDate = DateTime.Now;

                user.Status = UserStatus.Normal;

                user.Role = int.Parse(_settingService.GetSetting("DefaultRole"));

                _userRepository.Add(user);
            }
        }

        public User GetLoginUser()
        {
            return _sessionManager.User;
        }


        public void LogOut()
        {
            _sessionManager.User = null;
        }


        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
            if (_sessionManager.User.UserId == user.UserId)
            {
                _sessionManager.User = user;
            }
        }


        public void AddUser(User user)
        {
            if (_roleService.AdminPower())
            {
                if (_userRepository.CheckUserNameExist(user.UserName))
                {
                    throw new DomainException("UserName", "该用户名已经被注册。");
                }
                else if (_userRepository.CheckEmailExist(user.Email))
                {
                    throw new DomainException("Email", "该邮箱已经被注册。");
                }
                _userRepository.Add(user);
            }
            else
            {
                throw new DomainException("NoPower", "对不起，您没有权限进行操作。");
            }
        }


        public object Single(Func<System.Linq.IQueryable<User>, object> func)
        {
            return _userRepository.Single(func);
        }

        public System.Collections.Generic.IList<User> Find(Func<System.Linq.IQueryable<User>, System.Linq.IQueryable<User>> func)
        {
            return _userRepository.Find(func);
        }

        public void Delete(int id)
        {
            if (_roleService.AdminPower())
            {
                _userRepository.Delete(id);
            }
            else
            {
                throw new DomainException("NoPower", "对不起，您没有权限。");
            }
        }


        public User GetUserById(int id)
        {
            if (_roleService.AdminPower())
            {
                User user = _userRepository.Get(id);
                if (user == null)
                {
                    throw new DomainException("NoFind", "对不起，没有找到该用户。");
                }
                return user;
            }
            else
            {
                throw new DomainException("NoPower", "对不起，您没有权限。");
            }
        }
    }
}
