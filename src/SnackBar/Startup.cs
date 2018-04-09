using AutoMapper;
using Microsoft.Owin;
using Owin;
using SnackBar.BLL.Utils;

[assembly: OwinStartupAttribute(typeof(SnackBar.Startup))]

namespace SnackBar
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new BarProfile());
            });
        }
    }
}