using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Models
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
