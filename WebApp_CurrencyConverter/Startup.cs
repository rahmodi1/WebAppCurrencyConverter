using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApp_CurrencyConverter.Startup))]
namespace WebApp_CurrencyConverter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
