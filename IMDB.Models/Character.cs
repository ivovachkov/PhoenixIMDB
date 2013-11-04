using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Imdb.Models
{
    public class Character
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public virtual Movie Movie { get; set; }

        public virtual Person Actor { get; set; }
    }
}