using Fronius.Library.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronius.Library.Services.Services
{
    public interface IService : IDisposable
    {
    }
    public interface IService<T> where T : IService
    {
        T Repository { get; }
    }
}
