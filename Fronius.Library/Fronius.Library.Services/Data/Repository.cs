using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronius.Library.Services.Data
{
    public abstract class Repository<T, U> : IRepository<T, U>
        where U : DbContext, new()
        where T : class
    {
        private readonly U _repositoryContext;
        private DbSet<T> _objectSet;

        public DbSet<T> ObjectSet => _objectSet;

        protected Repository()
            : this(new U())
        {
        }

        protected Repository(U context)
        {
            context.Configuration.AutoDetectChangesEnabled = true;
            _repositoryContext = context;
            _objectSet = _repositoryContext.Set<T>();
        }

        public void Dispose()
        {
            _repositoryContext.Dispose();
            _objectSet = null;
        }
    }
}
