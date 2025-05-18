using Fronius.Library.API;
using Xunit;

namespace Fronius.Library.UnitTest
{
    public sealed class BookUnitTest
    {
        [Fact]
        public void TestPost()
        {
            BookController bookController = new BookController();

            Assert.Equal(-1, bookController.Post(new BookCreateModel() { Title = "Les Robots", ReleaseYear = 1950, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 5 } })); // TODO: API models
        }
    }
}