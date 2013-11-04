using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Imdb.Models
{
    public class Job
    {
        public Job()
        {
            this.People = new HashSet<Person>();
        }

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Person> People { get; set; }
    }
}