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
}
