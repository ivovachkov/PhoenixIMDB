using Imdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imdb.Mvc.Areas.Admin.ViewModels
{
    public class PersonTest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string BirthPlace { get; set; }

        public int BirthYear { get; set; }

        public string MoviesAsString { get; set; }

        public string JobsAsString { get; set; }

        public string ImageUrl { get; set; }

        //public virtual Image Image { get; set; }
    }
}