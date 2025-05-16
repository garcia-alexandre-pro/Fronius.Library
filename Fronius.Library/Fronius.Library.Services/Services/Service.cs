using System.Data.Entity;

namespace Fronius.Library.Services
{
    public abstract class Service<T, U> : IService<T, U>
        where U : DbContext, new()
        where T : class
    {
        private readonly U _context;
        private DbSet<T> _entitySet;

        public DbSet<T> EntitySet => _entitySet;

        public U Context => _context;

        protected Service()
            : this(new U())
        {
        }

        protected Service(U context)
        {
            //context.Configuration.AutoDetectChangesEnabled = true;
            _context = context;
            _entitySet = _context.Set<T>();
        }

        public void Dispose()
        {
            _context.Dispose();
            _entitySet = null;
        }
    }
}
