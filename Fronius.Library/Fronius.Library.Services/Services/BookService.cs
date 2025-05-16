using System.Collections.Generic;
using System.Linq;

namespace Fronius.Library.Services
{
    public sealed class BookService : Service<Book, LibraryEntities>
    {
        public IEnumerable<Book> Get(int? authorId = null)
        {
            return EntitySet;
                //.Where(x => x.Authors.Select(z => z.Id).Contains(authorId))
                //.OrderBy();
        }

        /// <summary>
        /// Inserts a row into the Book table in the database
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public int Add(Book book)
        {
            EntitySet.Add(book);

            Context.SaveChanges();

            return book.Id;
        }
    }
}
