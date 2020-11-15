using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace IOT.MVCWebMis
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",//路由名称
                url: "{controller}/{action}/{id}",//带有参数的url
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }//参数默认值
            );

            //注册JObject对象的参数解析
            ModelBinders.Binders.Add(typeof(JObject), new JObjectModelBinder());
        }
    }
}