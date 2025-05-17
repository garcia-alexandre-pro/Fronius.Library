using Fronius.Library.Models;
using Fronius.Library.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fronius.Library.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        // GET: api/<BookController>/7
        [HttpGet("{authorId}")]
        public IEnumerable<BookListModel> Get(int? authorId = null, string? orderingColumn = null, string? orderingDirection = null) // TODO: direction as a string or a boolean?
        {
            object? column = null;
            object? direction = null;

            if (orderingColumn != null && Enum.TryParse(typeof(Constants.OrderingColumn), orderingColumn, out column)) {
                throw new ArgumentException("Invalid ordering column.", nameof(orderingColumn));
            }

            if (orderingDirection != null && Enum.TryParse(typeof(Constants.OrderingDirection), orderingDirection, out direction))
            {
                throw new ArgumentException("Invalid ordering direction.", nameof(orderingDirection));
            }

            using (BookService bookService = new BookService())
            {
                return bookService.Get(authorId, (Constants.OrderingColumn?)column, (Constants.OrderingDirection?)direction); // TODO: add parameters
            }
        }

        // POST api/<BookController>
        [HttpPost]
        public void Post([FromBody] string value) // TODO: parameters
        {
            using (BookService bookService = new BookService())
            {
                bookService.Add(new Models.BookCreateModel()); // TODO: populate model
            }
        }
    }
}
