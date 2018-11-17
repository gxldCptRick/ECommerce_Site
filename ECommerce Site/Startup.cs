using ECommerceSite.DAL.Services;
using ECommerceSite.DAL.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using System.Web.Services.Description;

[assembly: OwinStartupAttribute(typeof(ECommerce_Site.Startup))]
namespace ECommerce_Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IProductService>(new StaticProductService());
        }
    }
}
