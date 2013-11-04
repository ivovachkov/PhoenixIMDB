using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Imdb.Models
{
    public class Person
    {
        public Person()
        {
            this.Movies = new HashSet<Movie>();
            this.Jobs = new List<Job>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string BirthPlace { get; set; }

        public int BirthYear { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }

        public virtual Image Image { get; set; }
    }
}
