using Blog.Repository.Interface;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Repository
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private ISession _session;
        private IDatabase _database;
        private ISessionFactory _sessionFactory;
        private string _path;

        public DatabaseFactory(string path)
        {
            _path = path;
        }

        public IDatabase GetDatabase()
        {
            if (_session == null || !_session.IsConnected)
            {
                lock (this.GetType())
                {
                    if (_session == null || !_session.IsConnected)
                    {
                        _sessionFactory = new Configuration().Configure(_path + "Bin/hibernate.cfg.xml").BuildSessionFactory();
                        _session = _sessionFactory.OpenSession();
                        _database = new Database(_session);
                    }
                }
            }

            return _database;
        }

        public void Dispose()
        {
            _session.Dispose();
            _sessionFactory.Dispose();
        }

    }
}
