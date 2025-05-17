using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronius.Library.Services
{
    internal class AuthorService : Service<Author, LibraryEntities>
    {
        public List<Author> Get(int[] ids)
        {
            return EntitySet.Where(x => ids.Contains(x.Id)).ToList();
        }
    }
}
