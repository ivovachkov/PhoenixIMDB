using Imdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Imdb.Mvc.Areas.Admin.ViewModels
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
                };
            }
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public string ActorsAsString { get; set; }
        public string WritersAsString { get; set; }
        public string DirectorsAsString { get; set; }
        public string CategoriesAsString { get; set; }
        public string ReviewsAsString { get; set; }
        public string CharactersAsString { get; set; }
        public string ImageUrl { get; set; }
    }
}