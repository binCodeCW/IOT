using System.Web.Http;
using IOT.MVCWebMis.Controllers;

namespace IOT.MVCWebMis
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            config.MessageHandlers.Add(new CustomErrorMessageDelegatingHandler());
        }
    }
}
