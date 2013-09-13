using System;
using NHibernate;
using Blog.Repository.Interface;
using Blog.Model;

namespace Blog.Repository
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(ISession session)
            : base(session)
        {
        }
    }
}
