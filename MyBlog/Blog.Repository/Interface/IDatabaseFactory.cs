using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repository.Interface
{
    public interface IDatabaseFactory:IDisposable
    {
        IDatabase GetDatabase();
    }
}
