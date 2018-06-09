using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CertficatesSocial.Startup))]
namespace CertficatesSocial
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
