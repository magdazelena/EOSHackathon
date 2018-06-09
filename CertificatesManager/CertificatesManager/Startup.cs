using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CertificatesManager.Startup))]
namespace CertificatesManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
