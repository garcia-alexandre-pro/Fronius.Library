using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronius.Library.Services
{
    public sealed class BookService : Service<Book, LibraryEntities>
    {
        public List<Book> Get()
        {
            return EntitySet.ToList();
        }

        public int Add(Book book)
        {
            EntitySet.Add(book);

            Context.SaveChanges();

            return book.Id;
        }
    }
}
