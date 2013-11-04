using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Imdb.Models
{
    public class Category
    {
        public Category()
        {
            this.Movies = new HashSet<Movie>();
        }

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
