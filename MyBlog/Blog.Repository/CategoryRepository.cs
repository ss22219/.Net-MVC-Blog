using System;
using NHibernate;
using Blog.Repository.Interface;
using Blog.Model;

namespace Blog.Repository
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(ISession session)
            : base(session)
        {
        }

        public System.Collections.Generic.IList<string> GetMonthCategory()
        {
            //string sql = "select SUBSTRING( CONVERT(varchar,CreateDate,112),0,7) from Article Group by SUBSTRING( CONVERT(varchar,CreateDate,112),0,7)";
            string sql = "SELECT LEFT( CreateDate, 7 ) AS lefttime FROM Article GROUP BY lefttime";
            var query = session.CreateSQLQuery(sql);
            var list = query.List<string>();

            for (int i = 0; i < list.Count; i++)
            {
                list[i] = list[i].Replace("-", string.Empty);
            }
            return list;
        }
    }
}
