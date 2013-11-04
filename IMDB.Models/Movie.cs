using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Models
{
    public class Movie
    {
        public Movie()
        {
            this.Categories = new HashSet<Category>();
            this.Actors = new HashSet<Actor>();
            this.Writers = new HashSet<Writer>();
            this.Directors = new HashSet<Director>();
            this.Reviews = new HashSet<Review>();
            this.Characters = new HashSet<Character>();
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Actor> Actors { get; set; }

        public virtual ICollection<Writer> Writers { get; set; }

        public virtual ICollection<Director> Directors { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Character> Characters { get; set; }

        public virtual Image Poster { get; set; }
    }
}
