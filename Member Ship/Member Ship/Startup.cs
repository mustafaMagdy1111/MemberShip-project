using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Member_Ship.Startup))]
namespace Member_Ship
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
