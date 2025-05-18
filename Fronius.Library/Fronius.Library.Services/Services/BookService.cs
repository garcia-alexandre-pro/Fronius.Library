using Fronius.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Fronius.Library.Services
{
    public sealed class BookService : Service<Book, LibraryEntities>
    {
        private const int ISBN_LENGTH = 13;

        /// <summary>
        /// Gets all the books, or only a filtered result set.
        /// </summary>
        /// <param name="authorId">The author, if <c>null</c>, returning all the books, else, returning only the books for which they are the author.</param>
        /// <param name="orderingColumn">The column by which the result set is ordered.</param>
        /// <param name="orderingDirection">The ordering direction.</param>
        /// <returns>The books formated data.</returns>
        public IEnumerable<BookListModel> Get(int? authorId, Constants.OrderingColumn? orderingColumn, Constants.OrderingDirection? orderingDirection)
        {
            if (orderingColumn == null)
            {
                orderingDirection = null;
            }
            else if (orderingDirection == null) // if orderingColumn has value and orderingDirection has not
            {
                orderingDirection = Constants.OrderingDirection.asc;
            }

            return Context.GetBooks(authorId, orderingColumn?.ToString(), orderingDirection?.ToString())
                .Select(x => new BookListModel(x)); // TODO: test validation (invalid author identifier, invalid ordering parameters...)
        }

        /// <summary>
        /// Inserts a row into the Book table in the database.
        /// </summary>
        /// <param name="book">The book data to insert.</param>
        /// <returns>The newly inserted book identifier.</returns>
        public int Add(BookCreateModel book)
        {
            if (EntitySet.Any(x => x.Title == book.Title.Trim().ToLowerInvariant()
                && x.ReleaseYear == book.ReleaseYear
                && !x.Authors.Select(z => z.Id).Except(book.Authors).Any()
                && !book.Authors.Except(x.Authors.Select(z => z.Id)).Any()))
            {
                return -1;
            }

            if (book.Title == null || book.Title.Trim() == string.Empty)
            {
                return -2;
            }

            if (book.ReleaseYear < 1450)
            {
                return -3;
            }

            Regex regex = new Regex($"^\\d{{{ISBN_LENGTH}}}$");

            if (book.ReleaseYear < 1970 && book.ISBN != null
                || book.ReleaseYear >= 1970 && (book.ISBN == null || book.ISBN.Length != ISBN_LENGTH || !regex.IsMatch(book.ISBN)))
            {
                return -3;
            }

            try
            {
                Book newBook = new Book()
                {
                    Title = book.Title.Trim(),
                    ReleaseYear = book.ReleaseYear,
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
            catch (Exception) // TODO: filter by excpetion type
            {
                return -4;
            }
        }
    }
}
