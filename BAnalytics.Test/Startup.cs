using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BAnalytics.Test.Startup))]
namespace BAnalytics.Test
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
