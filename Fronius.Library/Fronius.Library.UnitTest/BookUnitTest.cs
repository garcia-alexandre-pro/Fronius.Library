using Fronius.Library.API;
using Fronius.Library.API.Controllers;
using Fronius.Library.Services;

namespace Fronius.Library.UnitTest
{
    public sealed class BookUnitTest
    {
        [Fact]
        public void TestPost()
        {
            BookController bookController = new BookController(new BookService());

            //Assert.Equal(-1, bookController.Post());
        }

        [Fact]
        public void TestGet()
        {

        }
    }
}