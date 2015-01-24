using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Schedulator.Startup))]
namespace Schedulator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
