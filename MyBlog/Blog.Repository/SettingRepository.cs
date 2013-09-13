using System;
using NHibernate;
using Blog.Repository.Interface;
using Blog.Model;

namespace Blog.Repository
{
    public class SettingRepository:RepositoryBase<Setting>,ISettingRepository
    {
        public SettingRepository(ISession session)
            : base(session)
        {
        }
    }
}
