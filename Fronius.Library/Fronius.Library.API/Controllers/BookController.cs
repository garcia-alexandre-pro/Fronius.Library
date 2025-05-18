using Fronius.Library.Models;
using Fronius.Library.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fronius.Library.API
{
    public class BookController : ApiController
    {
        // GET: api/book/7
        public IEnumerable<BookListModel> Get(int? authorId = null, string orderingColumn = null, string orderingDirection = null) // TODO: direction as a string or a boolean?
        {
            Constants.OrderingColumn? column = null;
            Constants.OrderingDirection? direction = null;

            if (orderingColumn != null)
            {
                if (!Enum.TryParse(orderingColumn, out Constants.OrderingColumn columnTemp))
                {
                    throw new ArgumentException("Invalid ordering column.", nameof(orderingColumn));
                }

                column = columnTemp;
            }

            if (orderingDirection != null)
            {
                if (!Enum.TryParse(orderingDirection, out Constants.OrderingDirection directionTemp))
                {
                    throw new ArgumentException("Invalid ordering direction.", nameof(orderingDirection));
                }

                direction = directionTemp;
            }

            using (BookService bookService = new BookService())
            {
                return bookService.Get(authorId, column, direction);
            }
        }

        // POST api/book
        public int Post([FromBody] BookCreateModel book)
        {
            if (!ModelState.IsValid) {
                return -5;
            }

            using (BookService bookService = new BookService())
            {
                return Math.Min(bookService.Add(book), 0); // hiding identifier
            }
        }
    }
}
