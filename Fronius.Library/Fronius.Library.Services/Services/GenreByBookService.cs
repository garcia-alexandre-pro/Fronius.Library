using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronius.Library.Services
{
    internal class GenreByBookService : Service<GenreByBook, LibraryEntities>
    {
        public bool Add(int bookId, short[] genreIds)
        {
            foreach (short genreId in genreIds)
            {
                EntitySet.Add(new GenreByBook() { BookId = bookId, GenreId = genreId });
            }

            return Context.SaveChanges() > 0;
        }
    }
}
