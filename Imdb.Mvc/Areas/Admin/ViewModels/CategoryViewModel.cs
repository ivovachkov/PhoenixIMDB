using Imdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Imdb.Mvc.Areas.Admin.ViewModels
{
    public class CategoryViewModel
    {
        public static Expression<Func<Category, CategoryViewModel>> FromCategory
        {
            get
            {
                return cat => new CategoryViewModel
                {
                    Id = cat.Id,
                    Name = cat.Name
                };
            }
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}