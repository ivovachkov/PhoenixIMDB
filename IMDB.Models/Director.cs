using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Models
{
    public class Director
    {
        public int Id { get; set; }

        public virtual Person Person { get; set; }
    }
}
