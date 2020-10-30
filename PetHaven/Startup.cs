using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PetHaven.Startup))]
namespace PetHaven
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
