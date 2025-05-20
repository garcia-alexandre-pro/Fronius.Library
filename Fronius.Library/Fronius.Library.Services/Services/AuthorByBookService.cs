using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronius.Library.Services
{
    internal class AuthorByBookService : Service<AuthorByBook, LibraryEntities>
    {
        public bool Add(int bookId, int[] authorIds)
        {
            foreach (int authorId in authorIds)
            {
                EntitySet.Add(new AuthorByBook() { BookId = bookId, AuthorId = authorId });
            }

            return Context.SaveChanges() > 0;
        }
    }
}
