using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MedicalTansik.Startup))]
namespace MedicalTansik
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
