using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronius.Library.Services
{
    internal class GenreService : Service<Genre, LibraryEntities>
    {
        public List<Genre> Get(short[] ids)
        {
            return EntitySet.Where(x => ids.Contains(x.Id)).ToList();
        }
    }
}
