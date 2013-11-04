using Imdb.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Data
{
    public class ImdbContext : IdentityDbContextWithCustomUser<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
