using Fronius.Library.Models;
using Fronius.Library.Services;
using System;
using Xunit;

namespace Fronius.Library.UnitTest
{
    public sealed class ServicesUnitTest
    {
        [Fact]
        public void TestValidation()
        {
            using (BookService bookService = new BookService())
            {
                Assert.Equal(-1, bookService.Add(new BookCreateModel() { Title = "Les Robots", ReleaseYear = 1950, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 5 } }));
                
                Assert.Equal(-2, bookService.Add(new BookCreateModel() { Title = null, ReleaseYear = 1900, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 2 } }));
                Assert.Equal(-2, bookService.Add(new BookCreateModel() { Title = "", ReleaseYear = 1900, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 2 } }));
                Assert.Equal(-2, bookService.Add(new BookCreateModel() { Title = " ", ReleaseYear = 1900, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 2 } }));
                
                Assert.Equal(-3, bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = 1449, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 2 } }));

                Assert.Equal(-4, bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = 1900, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 1, Genres = new short[] { 1, 2 } }));
                Assert.Equal(-4, bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = 1900, ISBN = null, Authors = new int[] { 5 }, IllustratorId = 5, Genres = new short[] { 1, 2 } }));
                Assert.Equal(-4, bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = 1900, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 10 } }));

                //Assert.InRange(bookService.Add(new BookCreateModel() { Title = "Test", ReleaseYear = 1900, ISBN = null, Authors = new int[] { 1 }, IllustratorId = 5, Genres = new short[] { 1, 2 } }), 1, Int32.MaxValue);
            }
        }
    }
}