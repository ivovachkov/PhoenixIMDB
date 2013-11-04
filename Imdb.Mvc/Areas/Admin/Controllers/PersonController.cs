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
using Imdb.Mvc.Areas.Admin.ViewModels;

namespace Imdb.Mvc.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class PersonController : Controller
    {
        private IUowData db = new UowData();

        public ActionResult Index()
        {
            var people = from p in db.People.All()
                         select new PersonViewModel
                         {
                             Id = p.Id,
                             BirthYear = p.BirthYear,
                             Name = p.Name,
                             ImageUrl = p.Image.Url,
                             BirthPlace = p.BirthPlace,
                             JobsAsString = string.Join(", ", p.Jobs),
                             Jobs = p.Jobs,
                             MoviesAsString = string.Join(", ", p.Movies),
                             Movies = p.Movies,
                         };

            return View(people);
        }

        public JsonResult GetJobs([DataSourceRequest]DataSourceRequest request)
        {
            var jobs = db.Jobs.All().Select(JobViewModel.FromJob);
            return Json(jobs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var people = db.People.All().ToList();
            List<PersonViewModel> peopleVM = new List<PersonViewModel>();

            foreach (var person in people)
            {
                peopleVM.Add(new PersonViewModel()
                    {
                        Id = person.Id,
                        BirthYear = person.BirthYear,
                        Description = person.Description,
                        Name = person.Name,
                        ImageUrl = (person.Image == null) ? "no image" : person.Image.Url,
                        BirthPlace = person.BirthPlace,
                        JobsAsString = string.Join(", ", person.Jobs.Select(x => x.Name)),
                        MoviesAsString = string.Join(", ", person.Movies.Select(x => x.Title)),
                    });
            }

            DataSourceResult result = peopleVM.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, PersonTest person,
            Dictionary<string, object>[] jobs)
        {
            if (ModelState.IsValid)
            {
                var currPerson = new Person
                {
                    Name = person.Name,
                    BirthPlace = person.BirthPlace,
                    BirthYear = person.BirthYear,
                    Description = person.Description,
                    Image = new Image()
                    {
                        Url = person.ImageUrl
                    }
                };
                
                if (jobs != null)
                {
                    foreach (var job in jobs)
                    {
                        foreach (var obj in job)
                        {
                            if (obj.Key == "Id")
                            {
                                int jobId = Convert.ToInt32(((string[])obj.Value)[0]);
                                var currJob = db.Jobs.GetById(jobId);
                                currPerson.Jobs.Add(currJob);
                            }
                        }
                    }
                }

                db.People.Add(currPerson);
                db.SaveChanges();

                person.JobsAsString = string.Join(", ", currPerson.Jobs.Select(x => x.Name));
            }

            return Json(new[] { person }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request,
            PersonTest person, Dictionary<string, object>[] jobs)
        {
            if (ModelState.IsValid)
            {
                var currPerson = db.People.GetById(person.Id);
                currPerson.Name = person.Name;
                currPerson.BirthYear = person.BirthYear;
                currPerson.BirthPlace = person.BirthPlace;
                currPerson.Description = person.Description;
                currPerson.Image = new Image()
                {
                    Url = person.ImageUrl
                };

                var personJobs = new HashSet<Job>();

                if (jobs != null)
                {
                    foreach (var job in jobs)
                    {
                        foreach (var obj in job)
                        {
                            if (obj.Key == "Id")
                            {

                                int jobId = Convert.ToInt32(((string[])obj.Value)[0]);
                                var currJob = db.Jobs.GetById(jobId);
                                personJobs.Add(currJob);
                            }
                        }
                    }

                    if (personJobs.Count <= currPerson.Jobs.Count)
                    {
                        currPerson.Jobs = personJobs;
                    }
                    else
                    {
                        foreach (var job in personJobs)
                        {
                            if (!currPerson.Jobs.Contains(job))
                            {
                                currPerson.Jobs.Add(job);
                            }
                        }
                    }
                }
                else
                {
                    currPerson.Jobs = personJobs;
                }

                db.SaveChanges();

                person.JobsAsString = string.Join(", ", personJobs.Select(x => x.Name));
            }

            return Json(new[] { person }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, Person person)
        {
            if (ModelState.IsValid)
            {
                db.People.Delete(person.Id);
                db.SaveChanges();
            }

            return Json(new[] { person }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
