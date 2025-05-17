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
            using (BookService bookService = new BookService())
            {
                return bookService.Get(authorId); // TODO: add parameters
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
