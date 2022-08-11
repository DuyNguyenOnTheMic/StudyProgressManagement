using Microsoft.Owin;
using Owin;

namespace StudyProgressManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
