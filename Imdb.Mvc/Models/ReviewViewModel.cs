using Imdb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Imdb.Mvc.Models
{
    public class ReviewViewModel
    {
        public static Expression<Func<Review, ReviewViewModel>> FromReview
        {
            get 
            {
                return r => new ReviewViewModel
                {
                    Id = r.Id,
                    Title = r.Title,
                    Content = r.Content,
                    Rating = r.Rating, 
                    MovieId = r.MovieId,
                    AuthorName = r.User.UserName
                };
            }
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int Rating { get; set; }

        public int MovieId { get; set; }

        public string AuthorName { get; set; }
    }
}