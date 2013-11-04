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

namespace Imdb.Mvc.Areas.Admin.Controllers
{
    [Authorize(Roles="admin")]
    public class CategoriesController : Controller
    {
        private IUowData db = new UowData();
        
        public ActionResult Index()
        {
            return View(db.Categories.All());
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Category> categories = db.Categories.All();
            DataSourceResult result = categories.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, Category Category)
        {
            if (ModelState.IsValid)
            {
                var entity = new Category
                {
                    Name = Category.Name,
                };

                db.Categories.Add(entity);
                db.SaveChanges();
                Category.Name = entity.Name;
            }

            return Json(new[] { Category }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, Category category)
        {
            if (ModelState.IsValid)
            {
                var currCategory = db.Categories.GetById(category.Id);
                currCategory.Name = category.Name;
                db.SaveChanges();
            }

            return Json(new[] { category }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Delete(category.Id);
                db.SaveChanges();
            }

            return Json(new[] { category }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
