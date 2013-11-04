using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Models
{
    public class ApplicationUser : User
    {
        public ApplicationUser()
        {
            this.Reviews = new HashSet<Review>();
        }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
