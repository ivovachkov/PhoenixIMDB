using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Imdb.Models;
using Imdb.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Imdb.Mvc.Models;

namespace Imdb.Mvc.Controllers
{
    public class ReviewsController : Controller
    {
        private IUowData db = new UowData();

        public ActionResult Index()
        {
            ViewBag.Reviews = db.Reviews.All().Where(x => x.User.UserName == User.Identity.Name).ToList();

            var reviews = from r in db.Reviews.All()
                          select new ReviewViewModel
                          {
                              Id = r.Id,
                              Title = r.Title,
                              Content = r.Content,
                              Rating = r.Rating,
                              MovieTitle = r.Movie.Title,
                              AuthorUserName = r.User.UserName
                          };

            return View(reviews);
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var reviews = db.Reviews.All().Where(x => x.User.UserName == User.Identity.Name).ToList();
            List<ReviewViewModel> reviewVM = new List<ReviewViewModel>();

            foreach (var review in reviews)
            {
                reviewVM.Add(new ReviewViewModel()
                {
                    Id = review.Id,
                    Title = review.Title,
                    Content = review.Content,
                    Rating = review.Rating,
                    MovieTitle = review.Movie.Title,
                    AuthorUserName = review.User.UserName
                });
            }

            DataSourceResult result = reviewVM.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, ReviewViewModel review)
        {
            if (ModelState.IsValid)
            {
                var currReview = db.Reviews.GetById(review.Id);
                currReview.Title = review.Title;
                currReview.Content = review.Content;
                currReview.Rating = review.Rating;

                db.SaveChanges();
            }

            return Json(new[] { review }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Review review)
        {
            if (ModelState.IsValid)
            {
                var currUser = db.Users.All().Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
                review.UserId = currUser.Id;
                db.Reviews.Add(review);

                db.SaveChanges();
                return RedirectToAction("Details/" + review.Id);
            }

            return View(review);
        }

        public ActionResult CreateAjax()
        {
            return PartialView("_ReviewCreateEdit");
        }

        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, Person review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Delete(review.Id);
                db.SaveChanges();
            }

            return Json(new[] { review }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.GetById((int)id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
