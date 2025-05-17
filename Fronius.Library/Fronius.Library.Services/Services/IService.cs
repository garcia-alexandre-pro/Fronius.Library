using System;
using System.Data.Entity;

namespace Fronius.Library.Services
{
    public interface IService : IDisposable
    {
    }

    public interface IService<T, U> : IService
        where U : DbContext, new()
        where T : class
    {
        /// <summary>
        /// Virtual set of entities from the database.
        /// </summary>
        DbSet<T> EntitySet { get; }
        
        /// <summary>
        /// Database context.
        /// </summary>
        U Context { get; }
    }
}
