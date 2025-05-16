using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronius.Library.Services.Data
{
    public interface IRepository : IDisposable
    {
    }

    public interface IRepository<T, U> : IRepository
        where U : DbContext, new()
        where T : class
    {
        DbSet<T> ObjectSet { get; }
    }
}
