using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BPLLCWEB.Startup))]
namespace BPLLCWEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
