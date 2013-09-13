using Blog.Repository.Interface;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public class Database:IDatabase
    {
        public ISession Session;
        public Database(ISession session)
        {
            Session = session;
        }

        public void Dispose()
        {
            Session.Close();
        }
    }
}
