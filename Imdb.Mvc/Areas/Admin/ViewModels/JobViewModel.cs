 using Imdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Imdb.Mvc.Areas.Admin.ViewModels
{
    public class JobViewModel
    {
        public static Expression<Func<Job, JobViewModel>> FromJob
        {
            get 
            {
                return job => new JobViewModel 
                {
                    Id = job.Id,
                    Name = job.Name
                };
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}