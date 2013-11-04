using Imdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Imdb.Mvc.Areas.Admin.ViewModels
{
    public class PersonViewModel
    {
        public static Expression<Func<Person, PersonViewModel>> FromPerson
        {
            get
            {
                return p => new PersonViewModel
                {
                    Id = p.Id,
                    Name = p.Name
                };
            }
        }

        public PersonViewModel()
        {
            this.Movies = new HashSet<Movie>();
            this.Jobs = new List<Job>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string BirthPlace { get; set; }

        public int BirthYear { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

        public string MoviesAsString { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }

        public string JobsAsString { get; set; }

        public string ImageUrl { get; set; }
    }
}