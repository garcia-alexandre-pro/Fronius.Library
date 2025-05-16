using Fronius.Library.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronius.Library.Services.Services
{
    public class BookService : Service<BookRepository>
    {
        public List<Book> GetBooks()
        {
            return Repository.ObjectSet.ToList();
        }
    }
}
