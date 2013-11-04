using Imdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Data
{
    public interface IUowData : IDisposable
    {
        IRepository<ApplicationUser> Users { get; }

        IRepository<Image> Images { get; }

        IRepository<Character> Characters { get; }

        IRepository<Review> Reviews { get; }

        IRepository<Category> Categories { get; }

        IRepository<Job> Jobs { get; }

        IRepository<Person> People { get; }

        IRepository<Actor> Actors { get; }

        IRepository<Writer> Writers { get; }

        IRepository<Director> Directors { get; }

        IRepository<Movie> Movies { get; }

        int SaveChanges();
    }
}
