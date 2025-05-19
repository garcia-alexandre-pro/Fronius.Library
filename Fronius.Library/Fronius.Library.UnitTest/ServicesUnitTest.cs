using Fronius.Library.Models;
using Fronius.Library.Services;
using System;
using Xunit;

namespace Fronius.Library.UnitTest
{
    public sealed class ServicesUnitTest
    {
        [Fact]
        public void TestTitleValidation()
        {
            using (BookService bookService = new BookService())
            {
                Assert.Equal(-1, bookService.Add(new BookCreateModel() { Title = null, ReleaseYear = 1900, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 2 } })); // Title is null
                Assert.Equal(-1, bookService.Add(new BookCreateModel() { Title = "", ReleaseYear = 1900, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 2 } })); // Title is empty
                Assert.Equal(-1, bookService.Add(new BookCreateModel() { Title = " ", ReleaseYear = 1900, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 2 } })); // Title is trimmed empty
            }
        }

        [Fact]
        public void TestReleaseYearValidation()
        {
            using (BookService bookService = new BookService())
            {
                Assert.Equal(-2, bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = 1449, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 2 } })); // Released before 1450
                Assert.Equal(-2, bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = (short)(DateTime.Today.Year + 1), ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 2 } })); // released in the future (after current year)
            }
        }

        [Fact]
        public void TestISBNValidation()
        {
            using (BookService bookService = new BookService())
            {
                Assert.Equal(-3, bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = 1900, ISBN = "0123456789012", Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 2 } })); // Has an ISBN before 1970
                Assert.Equal(-3, bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = 2000, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 2 } })); // Has no ISBN after 1970
                Assert.Equal(-3, bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = 2000, ISBN = "0123456789", Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 2 } })); // ISBN has wrong length
                Assert.Equal(-3, bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = 2000, ISBN = "0123456789abc", Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 2 } })); // ISBN has illegal characters
            }
        }

        [Fact]
        public void TestUnicityValidation()
        {
            using (BookService bookService = new BookService())
            {
                Assert.Equal(-4, bookService.Add(new BookCreateModel() { Title = "Les Robots", ReleaseYear = 1950, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 5 } })); // Already exists
            }
        }

        [Fact]
        public void TestForeignKeysValidation()
        {
            using (BookService bookService = new BookService())
            {
                Assert.Equal(-5, bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = 1900, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 1, Genres = new short[] { 1, 2 } })); // Illustrator does not exist
                Assert.Equal(-5, bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = 1900, ISBN = null, Authors = new int[] { 5 }, IllustratorId = 5, Genres = new short[] { 1, 2 } })); // Author does not exist
                Assert.Equal(-5, bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = 1900, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 10 } })); // Genre does not exist
                Assert.Equal(-5, bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = 2000, ISBN = "9782226019431", Authors = new int[] { 1, 2 }, IllustratorId = 5, Genres = new short[] { 1, 3 } })); // ISBN already exists
            }
        }

        //[Fact]
        //public void TestInsertion()
        //{
        //    using (BookService bookService = new BookService())
        //    {
        //        //Assert.InRange(bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = 1900, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 2 } }), 1, Int32.MaxValue);
        //    }
        //}
    }
}