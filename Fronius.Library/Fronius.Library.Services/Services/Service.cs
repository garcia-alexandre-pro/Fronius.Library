using System.Data.Entity;

namespace Fronius.Library.Services
{
    /// <summary>
    /// Service base class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public abstract class Service<T, U> : IService<T, U>
        where U : DbContext, new()
        where T : class
    {
        private readonly U _context;
        private DbSet<T> _entitySet;

        /// <summary>
        /// Database context.
        /// </summary>
        protected U Context => _context;

        /// <summary>
        /// Virtual set of entities from the database.
        /// </summary>
        internal DbSet<T> EntitySet => _entitySet;

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
