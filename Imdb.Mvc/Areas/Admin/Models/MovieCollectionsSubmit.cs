using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imdb.Mvc.Areas.Admin.Models
{
    public class MovieCollectionsSubmit
    {
        public Dictionary<string, object>[] Writers { get; set; }
        public Dictionary<string, object>[] Directors { get; set; }
        public Dictionary<string, object>[] Actors { get; set; }
        public Dictionary<string, object>[] Categories { get; set; }
    }
}