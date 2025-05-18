using Fronius.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronius.Library.Models
{
    public class BookCreateModel
    {
        public string Title { get; set; }
        public short ReleaseYear { get; set; }
        public string ISBN { get; set; }
        public int IllustratorId { get; set; }
        public int[] Authors { get; set; }
        public short[] Genres { get; set; }
    }

    public class BookListModel
    {
        public BookListModel(GetBooks_Result book)
        {
            Id = book.Id;
            Title = book.Title;
            ReleaseYear = book.Year;
            ISBN = book.ISBN;
            Authors = book.AuthorNames;
            Illustrator = book.IllustratorName;
            Genres = book.GenreNames;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public short ReleaseYear { get; set; }
        public string ISBN { get; set; }
        public string Authors { get; set; }
        public string Illustrator { get; set; }
        public string Genres { get; set; }
    }
}
