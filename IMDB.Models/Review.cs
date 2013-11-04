using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Imdb.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int Rating { get; set; }

        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; }

        public string UserId { get; set; }
        
        public virtual ApplicationUser User { get; set; }
    }
}
