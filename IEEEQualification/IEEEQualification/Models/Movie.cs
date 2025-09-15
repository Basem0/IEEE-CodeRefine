using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEEEQualification.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Author { get; set; } = "";
        public string Category { get; set; } = "";

        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<WatchlistItem> WatchlistItems { get; set; } = new List<WatchlistItem>();

    }
}
