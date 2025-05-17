using Fronius.Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace Fronius.Library.Services
{
    public sealed class BookService : Service<Book, LibraryEntities>
    {
        /// <summary>
        /// Gets all the books, or only a filtered result set.
        /// </summary>
        /// <param name="authorId">The author, if <c>null</c>, returning all the books, else, returning only the books for which they are the author.</param>
        /// <param name="orderingColumn">The column by which the result set is ordered.</param>
        /// <param name="orderingDirection">The ordering direction.</param>
        /// <returns>The books formated data.</returns>
        public IEnumerable<GetBooks_Result> Get(int? authorId = null, Constants.OrderingColumn? orderingColumn = null, Constants.OrderingDirection? orderingDirection = null)
        {
            return Context.GetBooks(authorId, orderingColumn?.ToString(), orderingDirection?.ToString());
        }

        /// <summary>
        /// Inserts a row into the Book table in the database.
        /// </summary>
        /// <param name="book">The book data to insert.</param>
        /// <returns>The newly inserted book identifier.</returns>
        public int Add(BookCreateModel book)
        {
            if (EntitySet.Any(x => x.Title == book.Title.Trim().ToLowerInvariant()
                && x.Year == book.ReleaseYear
                && x.Authors))
            {
                return -1;
            }

            Book newBook = new Book()
            {
                Title = book.Title.Trim(),
                Year = book.ReleaseYear,
                ISBN = book.ISBN,
                IllustratorId = book.IllustratorId
            };

            if (book.Authors.Any())
            {
                using (AuthorService authorService = new AuthorService())
                {
                    newBook.Authors = authorService.Get(book.Authors);
                }
            }

            if (book.Genres.Any())
            {
                using (GenreService genreService = new GenreService())
                {
                    newBook.Genres = genreService.Get(book.Genres);
                }
            }

            EntitySet.Add(newBook);

            Context.SaveChanges();

            return newBook.Id;
        }
    }
}
