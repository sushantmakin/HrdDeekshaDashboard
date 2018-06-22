using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HitaRasDharaDeekshaMissCallDashboard.Startup))]
namespace HitaRasDharaDeekshaMissCallDashboard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
