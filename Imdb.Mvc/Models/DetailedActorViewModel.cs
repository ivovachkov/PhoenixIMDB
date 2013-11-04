using Imdb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Imdb.Mvc.Models
{
    public class DetailedActorViewModel
    {
        public static Expression<Func<Actor, DetailedActorViewModel>> FromActor
        {
            get 
            {
                return x => new DetailedActorViewModel
                {
                    Id = x.Id,
                    Name = x.Person.Name,
                    Description = x.Person.Description,
                    ImageUrl = x.Person.Image.Url,
                    BirthPlace = x.Person.BirthPlace,
                    BirthYear = x.Person.BirthYear,
                    Movies = x.Person.Movies.Select(m => new CutMovieViewModel { Id = m.Id, Name = m.Title })
                };
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string BirthPlace { get; set; }

        public int BirthYear { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<CutMovieViewModel> Movies { get; set; }
    }
}