using Imdb.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Imdb.Models;
using Imdb.Mvc.Models;
using Microsoft.AspNet.Identity;
using System.Net;

namespace Imdb.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private IUowData db = new UowData();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReadMovies([DataSourceRequest] DataSourceRequest request)
        {
            var movies = db.Movies.All().Select(MovieViewModel.FromMovie);

            return Json(movies.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Movie(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var moviewFromDb = db.Movies.All()
                .Where(x => x.Id == id);

            var movie = moviewFromDb
                .Select(MovieViewModel.FromMovie)
                .FirstOrDefault();

            var userId = User.Identity.GetUserId();

            movie.UserCanVote = !moviewFromDb.FirstOrDefault()
                .Reviews.Any(x => x.UserId == userId);

            return View(movie);
        }

        public JsonResult GetAutoCompleteMovies(string text)
        {
            var selectedMovies = db.Movies.All()
                .Where(x => x.Title.ToLower()
                    .Contains(text.ToLower()))
                .Select(MovieViewModel.FromMovie);

            return Json(selectedMovies, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult PostReview(ReviewViewModel reviewModel)
        {
            if (ModelState.IsValid)
            {
                var username = this.User.Identity.GetUserName();
                var userId = this.User.Identity.GetUserId();

                var movie = db.Movies.GetById(reviewModel.MovieId);                
                var userCanVote = movie.Reviews.Any(x => x.UserId == userId);

                if (userCanVote)
                {
                    db.Reviews.Add(new Review
                    {
                        Content = reviewModel.Content,
                        Rating = reviewModel.Rating,
                        Title = reviewModel.Title,
                        UserId = userId,
                        MovieId = reviewModel.MovieId
                    });

                    db.SaveChanges();
                }                

                return PartialView("_Review", reviewModel);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ModelState.Values.First().ToString());
        }

        public ActionResult MyReviews()
        {
            var userId = this.User.Identity.GetUserId();

            var myReviews = db.Reviews.All()
                .Where(r => r.UserId == userId)
                .Select(ReviewViewModel.FromReview);

            return View(myReviews);
        }

        public ActionResult ActorDetails(int id)
        {
            var actor = db.Actors.All()
                .Where(x => x.Id == id)
                .Select(DetailedActorViewModel.FromActor)
                .FirstOrDefault();

            return View(actor);
        }
    }
}