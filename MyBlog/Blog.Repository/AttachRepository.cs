using System;
using NHibernate;
using Blog.Repository.Interface;
using Blog.Model;

namespace Blog.Repository
{
    public class AttachRepository : RepositoryBase<Attach>, IAttachRepository
    {
        public AttachRepository(ISession session)
            : base(session)
        {
        }
    }
}
