using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MusicApp.Startup))]
namespace MusicApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
