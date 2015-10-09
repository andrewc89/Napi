using System.Web.Mvc;
using System.Web.Routing;

namespace Napi.Example
{
    public class RouteConfig
    {
        public static void RegisterRoutes (RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("API/{*pathInfo}");
        }
    }
}