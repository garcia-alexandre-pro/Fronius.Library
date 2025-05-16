using System.Collections.Generic;
using System.Linq;

namespace Fronius.Library.Services
{
    public sealed class BookService : Service<Book, LibraryEntities>
    {
        public IEnumerable<GetBooks_Result> Get(int? authorId = null, Constants.OrderingColumn? orderingColumn = null, Constants.OrderingDirection? orderingDirection = null)
        {
            return Context.GetBooks(authorId, orderingColumn?.ToString(), orderingDirection?.ToString());
        }

        /// <summary>
        /// Inserts a row into the Book table in the database
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public int Add(Book book)
        {
            if(EntitySet.Any(x => x.Title == book.Title.Trim().ToLowerInvariant()
                && x.Year == book.Year))

            EntitySet.Add(book);

            Context.SaveChanges();

            return book.Id;
        }
    }
}
