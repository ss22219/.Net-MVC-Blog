using System;
using NHibernate;
using Blog.Repository.Interface;
using Blog.Model;
using System.Collections.Generic;

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
            IList<string> list = new List<string>();
            foreach (var obj in query.List())
            {
                string str = null;
                if (obj is string)
                {
                    str = (string)obj;
                }
                else
                {
                    str = System.Text.Encoding.Default.GetString((byte[])obj);
                }
                str = str.Replace("-", string.Empty);
                list.Add(str);
            }
            return list;
        }
    }
}
