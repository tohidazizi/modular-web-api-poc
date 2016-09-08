using System.Web.Http;

namespace ModularWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            GlobalConfiguration.Configuration.Services.Replace(
                typeof(System.Web.Http.Dispatcher.IHttpControllerSelector),
                new ModularHttpControllerSelector(config));

            config.MapHttpAttributeRoutes();
        }
    }
}
