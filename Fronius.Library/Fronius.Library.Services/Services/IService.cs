using System;
using System.Data.Entity;

namespace Fronius.Library.Services
{
    /// <summary>
    /// Service interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface IService<T, U> : IDisposable
        where U : DbContext, new()
        where T : class
    {
        ///// <summary>
        ///// Virtual set of entities from the database.
        ///// </summary>
        //DbSet<T> EntitySet { get; }
        
        ///// <summary>
        ///// Database context.
        ///// </summary>
        //U Context { get; }
    }
}
