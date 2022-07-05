using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudyProgressManagement.Startup))]
namespace StudyProgressManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.MapSignalR();
        }
    }
}
