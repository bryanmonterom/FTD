using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FTDWebSite.Startup))]
namespace FTDWebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
