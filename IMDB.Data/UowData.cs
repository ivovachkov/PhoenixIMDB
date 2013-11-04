using Imdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Data
{
    public class UowData : IUowData
    {
        private readonly ImdbContext context;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public UowData() : this(new ImdbContext())
        { }

        public UowData(ImdbContext context)
        {
            this.context = context;
        }

        public IRepository<ApplicationUser> Users
        {
            get { return this.GetRepository<ApplicationUser>(); }
        }

        public IRepository<Image> Images
        {
            get { return this.GetRepository<Image>(); }
        }

        public IRepository<Character> Characters
        {
            get { return this.GetRepository<Character>(); }
        }

        public IRepository<Review> Reviews
        {
            get { return this.GetRepository<Review>(); }
        }

        public IRepository<Category> Categories
        {
            get { return this.GetRepository<Category>(); }
        }

        public IRepository<Job> Jobs
        {
            get { return this.GetRepository<Job>(); }
        }

        public IRepository<Person> People
        {
            get { return this.GetRepository<Person>(); }
        }

        public IRepository<Actor> Actors
        {
            get { return this.GetRepository<Actor>(); }
        }

        public IRepository<Writer> Writers
        {
            get { return this.GetRepository<Writer>(); }
        }

        public IRepository<Director> Directors
        {
            get { return this.GetRepository<Director>(); }
        }

        public IRepository<Movie> Movies
        {
            get { return this.GetRepository<Movie>(); }
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
