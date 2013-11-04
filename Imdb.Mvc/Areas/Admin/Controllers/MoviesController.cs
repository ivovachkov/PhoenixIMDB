using Imdb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Imdb.Mvc.Areas.Admin.ViewModels;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Imdb.Models;
using Imdb.Mvc.Areas.Admin.Models;
using System.Reflection;

namespace Imdb.Mvc.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class MoviesController : Controller
    {
        private IUowData db = new UowData();

        public ActionResult Index()
        {
            var movies = from m in db.Movies.All()
                         select new MovieViewModel
                         {
                             Id = m.Id,
                             Title = m.Title,
                             Description = m.Description,
                             ImageUrl = m.Poster.Url,
                             Duration = m.Duration,
                             WritersAsString = string.Join(", ", m.Writers.Select(x => x.Person.Name)),
                             ActorsAsString = string.Join(", ", m.Actors.Select(x => x.Person.Name)),
                             DirectorsAsString = string.Join(", ", m.Directors.Select(x => x.Person.Name)),
                             CharactersAsString = string.Join(", ", m.Characters.Select(x => x.Name)),
                             CategoriesAsString = string.Join(", ", m.Categories.Select(x => x.Name)),
                         };

            return View(movies);
        }

        public JsonResult GetPeople([DataSourceRequest]DataSourceRequest request)
        {
            var writers = db.People.All().Select(PersonViewModel.FromPerson);
            return Json(writers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategories([DataSourceRequest] DataSourceRequest request)
        {
            var categories = db.Categories.All().Select(CategoryViewModel.FromCategory);
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var movies = db.Movies.All().ToList();
            List<MovieViewModel> movieVM = new List<MovieViewModel>();

            foreach (var movie in movies)
            {
                movieVM.Add(new MovieViewModel()
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Description = movie.Description,
                    ImageUrl = movie.Poster.Url,
                    Duration = movie.Duration,
                    WritersAsString = string.Join(", ", movie.Writers.Select(x => x.Person.Name)),
                    ActorsAsString = string.Join(", ", movie.Actors.Select(x => x.Person.Name)),
                    DirectorsAsString = string.Join(", ", movie.Directors.Select(x => x.Person.Name)),
                    CharactersAsString = string.Join(", ", movie.Characters.Select(x => x.Name)),
                    CategoriesAsString = string.Join(", ", movie.Categories.Select(x => x.Name)),
                });
            }

            DataSourceResult result = movieVM.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, MovieViewModel movie,
           MovieCollectionsSubmit collections)
        {
            if (ModelState.IsValid)
            {
                var currMovie = new Movie
                {
                    Title = movie.Title,
                    Description = movie.Description,
                    Duration = movie.Duration,
                    Poster = new Image()
                    {
                        Url = movie.ImageUrl
                    },
                };

                var writers = collections.Writers;
                var actors = collections.Actors;
                var directors = collections.Directors;
                var categories = collections.Categories;

                AddPerson(currMovie, writers, Type.GetType("Imdb.Models.Writer, IMDB.Models", true));
                AddPerson(currMovie, actors, Type.GetType("Imdb.Models.Actor, IMDB.Models", true));
                AddPerson(currMovie, directors, Type.GetType("Imdb.Models.Director, IMDB.Models", true));

                foreach (var cat in categories)
                {
                    var catId = Convert.ToInt32(((string[])cat.First().Value)[0]);
                    currMovie.Categories.Add(db.Categories.GetById(catId));
                    db.Categories.GetById(catId).Movies.Add(currMovie);
                }

                db.Movies.Add(currMovie);
                db.SaveChanges();

                movie.WritersAsString = string.Join(", ", currMovie.Writers.Select(x => x.Person.Name));
                movie.ActorsAsString = string.Join(", ", currMovie.Actors.Select(x => x.Person.Name));
                movie.DirectorsAsString = string.Join(", ", currMovie.Directors.Select(x => x.Person.Name));
                movie.CategoriesAsString = string.Join(", ", currMovie.Categories.Select(x => x.Name));
            }

            return Json(new[] { movie }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request,
            MovieViewModel movie, MovieCollectionsSubmit collections)
        {
            if (ModelState.IsValid)
            {
                var currMovie = db.Movies.GetById(movie.Id);
                currMovie.Title = movie.Title;
                currMovie.Description = movie.Description;
                currMovie.Duration = movie.Duration;
                currMovie.Description = movie.Description;

                var writers = collections.Writers;
                var actors = collections.Actors;
                var directors = collections.Directors;
                var categories = collections.Categories;

                currMovie.Writers = AddPeople<Writer>(writers, Type.GetType("Imdb.Models.Writer, IMDB.Models", true));
                currMovie.Actors = AddPeople<Actor>(actors, Type.GetType("Imdb.Models.Actor, IMDB.Models", true));
                currMovie.Directors = AddPeople<Director>(directors, Type.GetType("Imdb.Models.Director, IMDB.Models", true));
                currMovie.Categories = UpdateCategories(currMovie, categories);

                db.SaveChanges();

                movie.WritersAsString = string.Join(", ", currMovie.Writers.Select(x => x.Person.Name));
                movie.ActorsAsString = string.Join(", ", currMovie.Actors.Select(x => x.Person.Name));
                movie.DirectorsAsString = string.Join(", ", currMovie.Directors.Select(x => x.Person.Name));
                movie.CategoriesAsString = string.Join(", ", currMovie.Categories.Select(x => x.Name));
            }

            return Json(new[] { movie }.ToDataSourceResult(request, ModelState));
        }

        private HashSet<Category> UpdateCategories(Movie currMovie, Dictionary<string, object>[] categories)
        {
            HashSet<Category> cats = new HashSet<Category>();

            foreach (var cat in categories)
            {
                var catId = Convert.ToInt32(((string[])cat.First().Value)[0]);
                var currCat = db.Categories.GetById(catId);
                cats.Add(currCat);
                currCat.Movies.Add(currMovie);
            }
            return cats;
        }

        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, Movie movie)
        {
            ImdbContext context = new ImdbContext();
            if (ModelState.IsValid)
            {
                var currMovie = context.Movies.Find(movie.Id);
                context.Actors.RemoveRange(currMovie.Actors);
                context.Writers.RemoveRange(currMovie.Writers);
                context.Directors.RemoveRange(currMovie.Directors);
                context.Characters.RemoveRange(currMovie.Characters);
                context.Movies.Remove(currMovie);
                context.SaveChanges();
            }

            return Json(new[] { movie }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private HashSet<T> AddPeople<T>(Dictionary<string, object>[] people, Type typeOfPerson)
        {
            HashSet<T> collection = new HashSet<T>();
            if (people != null)
            {
                foreach (var person in people)
                {
                    int personId = Convert.ToInt32(((string[])person["Id"])[0]);
                    var currPerson = Activator.CreateInstance(typeOfPerson);
                    PropertyInfo thePerson = typeOfPerson.GetProperty("Person");
                    thePerson.SetValue(currPerson, db.People.GetById(personId));

                    collection.Add((T)currPerson);
                }
            }

            return collection;
        }

        private void AddPerson(Movie currMovie, Dictionary<string, object>[] people, Type typeOfPerson)
        {
            if (people != null)
            {
                foreach (var person in people)
                {
                    int personId = Convert.ToInt32(((string[])person["Id"])[0]);

                    var currPerson = Activator.CreateInstance(typeOfPerson);
                    PropertyInfo thePerson = typeOfPerson.GetProperty("Person");
                    thePerson.SetValue(currPerson, db.People.GetById(personId));
                    this.db.People.GetById(personId).Movies.Add(currMovie);

                    if (typeOfPerson.Name == "Writer")
                    {
                        currMovie.Writers.Add((Writer)currPerson);
                    }
                    else if (typeOfPerson.Name == "Actor")
                    {
                        currMovie.Actors.Add((Actor)currPerson);
                    }
                    else if (typeOfPerson.Name == "Director")
                    {
                        currMovie.Directors.Add((Director)currPerson);
                    }
                }
            }
        }
    }
}