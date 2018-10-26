using System.Web.Http;
using Api.S4Pay.Helper;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using S4Pay.CrossCutting;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Api.S4Pay
{
    public class Startup
    {
        public static void ConfigureWebApi(HttpConfiguration config)
        {
            var formatters = config.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            var jsonSettings = formatters.JsonFormatter.SerializerSettings;
            jsonSettings.Formatting = Formatting.Indented;
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
        }

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            ConfigureDependencyInjection(config, app);
            ConfigureWebApi(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
           
        public static void ConfigureDependencyInjection(HttpConfiguration config, IAppBuilder app)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            DependencyInjection.Configure(container);
            app.Use(async (context, next) =>
            {
                using (AsyncScopedLifestyle.BeginScope(container)) { await next(); }
            });
            config.DependencyResolver = new SimpleInjectorDependencyResolver(container);
            container.Verify();
        }
    }
}