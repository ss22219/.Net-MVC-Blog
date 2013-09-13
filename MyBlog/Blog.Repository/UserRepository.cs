using System;
using System.Collections.Generic;
using NHibernate;
using Blog.Repository.Interface;
using Blog.Model;

namespace Blog.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ISession session)
            : base(session)
        {
        }

        public bool CheckUserNameExist(string userName)
        {
            return session.CreateQuery(
                "from User where UserName=?"
                ).SetParameter(0, userName).List()
                .Count > 0;
        }

        public User CheckUser(string userName, string password)
        {
            IList<User> user = session.CreateQuery(
                "from User where UserName=? and Password=?")
                .SetParameter(0, userName).SetParameter(1, password).List<User>();
            if (user.Count > 0)
                return user[0];
            else
                return null;
        }

        public bool CheckEmailExist(string email)
        {
            return session.CreateQuery(
                "from User where Email=?")
                .SetParameter(0, email).List()
                .Count > 0;

        }
    }
}
