using Fronius.Library.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronius.Library.Services.Services
{
    public abstract class Service<T> : IService<T>
         where T : IRepository
    {
        private readonly T _repository;

        public T Repository => _repository;
    }
}
