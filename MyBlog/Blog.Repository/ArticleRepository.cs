using System;
using NHibernate;
using Blog.Repository.Interface;
using Blog.Model;

namespace Blog.Repository
{
    public class ArticleRepository : RepositoryBase<Article>, IArticleRepository
    {
        public ArticleRepository(ISession session)
            : base(session)
        {
        }
    }
}
