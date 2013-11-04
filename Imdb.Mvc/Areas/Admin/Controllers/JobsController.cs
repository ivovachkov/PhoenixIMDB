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
    public class JobsController : Controller
    {
        private IUowData db = new UowData();
      
        public ActionResult Index()
        {
            return View(db.Jobs.All());
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Job> jobs = db.Jobs.All();
            DataSourceResult result = jobs.Select(JobViewModel.FromJob).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, Job job)
        {
            if (ModelState.IsValid)
            {
                var entity = new Job
                {
                    Name = job.Name,
                };

                db.Jobs.Add(entity);
                db.SaveChanges();
                job.Name = entity.Name;
            }

            return Json(new[] { job }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, Job job)
        {
            if (ModelState.IsValid)
            {
                var currJob = db.Jobs.GetById(job.Id);
                currJob.Name = job.Name;
                db.SaveChanges();
            }

            return Json(new[] { job }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, Job job)
        {
            if (ModelState.IsValid)
            {
                db.Jobs.Delete(job.Id);
                db.SaveChanges();
            }

            return Json(new[] { job }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
