using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Blog.Repository.Interface;

namespace Blog.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ISession session;
        protected ITransaction transaction;

        public RepositoryBase(ISession session)
        {
            this.session = session;
        }

        public T Get(int id)
        {
            return session.Get<T>(id);
        }

        public System.Collections.Generic.IList<T> GetAll()
        {
            return session.QueryOver<T>().List();
        }

        public int Add(T entiry)
        {
            return (int)session.Save(entiry);
        }

        public void Delete(int id)
        {
            session.Delete(session.Get<T>(id));
        }

        public void Update(T entity)
        {
            session.Merge(entity);
        }


        public int Update(string sql, object[] args = null)
        {
            var query = session.CreateSQLQuery(sql);
            if (args != null)
            {
                for (int i = 0; i > args.Length; i++)
                {
                    query.SetParameter(i, args[i]);
                }
            }
            return query.ExecuteUpdate();
        }

        public void Save()
        {
            session.Flush();
        }

        public System.Collections.Generic.IList<T> Find(Func<IQueryable<T>, IQueryable<T>> expr)
        {
            return expr(session.Query<T>()).ToList();
        }

        public object Single(Func<IQueryable<T>, object> expr)
        {
            return expr(session.Query<T>());
        }

        public void BeginTransaction()
        {
            if (transaction != null && transaction.IsActive)
            {
                transaction.Commit();
                transaction.Dispose();
                transaction = null;
            }
            transaction = session.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            if (transaction != null && transaction.IsActive)
            {
                transaction.Rollback();
                transaction.Dispose();
                transaction = null;
            }
        }

        public void CommitTransaction()
        {
            if (transaction != null && transaction.IsActive)
            {
                transaction.Commit();
                transaction.Dispose();
                transaction = null;
            }
        }
    }
}
