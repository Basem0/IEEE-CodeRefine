using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEEEQualification.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; } 
        public string Content { get; set; } = "";

        public int UserId { get; set; }
        public User User { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

    }
}
