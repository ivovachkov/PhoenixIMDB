using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Imdb.Mvc.Startup))]
namespace Imdb.Mvc
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            ConfigureAuth(app);
        }
    }
}
