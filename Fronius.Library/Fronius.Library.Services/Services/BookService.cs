using Fronius.Library.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;

namespace Fronius.Library.Services
{
    public sealed class BookService : Service<Book, LibraryEntities>
    {
        private readonly object _obj = new object();
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
                .Select(x => new BookListModel(x)).ToList();
        }

        /// <summary>
        /// Inserts a row into the Book table in the database.
        /// </summary>
        /// <param name="book">The book data to insert.</param>
        /// <returns>The newly inserted book identifier.</returns>
        public int Add(BookCreateModel book)
        {
            lock (_obj) // avoid concurrent accesses from several threads
            {
                if (book.Authors == null || !book.Authors.Any() || book.Genres == null || !book.Genres.Any())
                {
                    return -1;
                }

                if (book.Title == null || book.Title.Trim() == string.Empty)
                {
                    return -2;
                }

                if (book.ReleaseYear < 1450 || book.ReleaseYear > DateTime.Today.Year)
                {
                    return -3;
                }

                Regex regex = new Regex($"^\\d{{{ISBN_LENGTH}}}$");

                if (book.ReleaseYear < 1970 && book.ISBN != null
                    || book.ReleaseYear >= 1970 && (book.ISBN == null || book.ISBN.Length != ISBN_LENGTH || !regex.IsMatch(book.ISBN) || EntitySet.Any(x => x.ISBN == book.ISBN)))
                {
                    return -4;
                }

                if (EntitySet.Any(x => x.Title.Trim().ToLower() == book.Title.Trim().ToLower()
                    && x.ReleaseYear == book.ReleaseYear
                    && !x.AuthorByBooks.Select(z => z.AuthorId).Except(book.Authors).Any()
                    && !book.Authors.Except(x.AuthorByBooks.Select(z => z.AuthorId)).Any()))
                {
                    return -5;
                }

                try
                {
                    using (DbContextTransaction transactionContext = Context.Database.BeginTransaction())
                    {
                        Book newBook = new Book()
                        {
                            Title = book.Title.Trim(),
                            ReleaseYear = book.ReleaseYear,
                            ISBN = book.ISBN,
                            IllustratorId = book.IllustratorId
                        };

                        EntitySet.Add(newBook);

                        Context.SaveChanges();

                        foreach (int authorId in book.Authors)
                        {
                            Context.AuthorByBooks.Add(new AuthorByBook()
                            {
                                BookId = newBook.Id,
                                AuthorId = authorId
                            });
                        }

                        foreach (short genreId in book.Genres)
                        {
                            Context.GenreByBooks.Add(new GenreByBook()
                            {
                                BookId = newBook.Id,
                                GenreId = genreId
                            });
                        }

                        Context.SaveChanges();

                        transactionContext.Commit();

                        return newBook.Id;
                    }
                }
                catch (DbUpdateException e)
                {
                    return -6;
                }
            }
        }
    }
}
