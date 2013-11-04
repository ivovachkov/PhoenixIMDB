using Imdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Imdb.Mvc.Models
{
    public class MovieViewModel
    {
        public static Expression<Func<Movie, MovieViewModel>> FromMovie
        {
            get
            {
                return movie => new MovieViewModel
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Description = movie.Description,
                    Duration = movie.Duration,
                    ImageUrl = movie.Poster.Url,
                    Reviews = movie.Reviews.AsQueryable().Select(ReviewViewModel.FromReview),
                    Actors = movie.Actors.Select(x => new ActorViewModel { Id = x.Id, Name = x.Person.Name })
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool UserCanVote { get; set; }

        public IEnumerable<ReviewViewModel> Reviews { get; set; }

        public IEnumerable<ActorViewModel> Actors { get; set; }
    }
}