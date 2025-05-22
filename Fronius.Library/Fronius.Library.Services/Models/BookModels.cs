using Fronius.Library.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronius.Library.Models
{
    /// <summary>
    /// Book creation model (POST).
    /// </summary>
    public class BookCreateModel
    {
        private int[] _authors;
        private short[] _genres;

        [Required(ErrorMessage = "Title required.")]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "Title should be between 1 and 250 characters.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Release year required.")]
        [Range(1450, 9999, ErrorMessage = "Release year should be between 1450 and current year.")]
        public short ReleaseYear { get; set; }
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ISBN should be 13 characters.")]
        public string ISBN { get; set; }
        [Required(ErrorMessage = "Illustrator required.")]
        public int IllustratorId { get; set; }
        [Required(ErrorMessage = "Authors required.")]
        public int[] Authors { get => _authors; set => _authors = value?.Distinct().ToArray(); }
        [Required(ErrorMessage = "Genres required.")]
        public short[] Genres { get => _genres; set => _genres = value?.Distinct().ToArray(); }
    }

    /// <summary>
    /// Book listing model (GET).
    /// </summary>
    public class BookListModel
    {
        public BookListModel(GetBooks_Result book)
        {
            Id = book.Id;
            Title = book.Title;
            ReleaseYear = book.ReleaseYear;
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
