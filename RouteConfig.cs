using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DifferingAPI2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

             routes.MapRoute(
                name: "PutLeft",
                url: "v1/diff/{id}/left",
                defaults: new { controller = "Home", action = "PutLeft", id = UrlParameter.Optional   }
            );
            routes.MapRoute(
             name: "PutRight",
             url: "v1/diff/{id}/right",
             defaults: new { controller = "Home", action = "PutRight", id = UrlParameter.Optional }
         );
            routes.MapRoute(
             name: "FetchResult",
             url: "v1/diff/{id}",
             defaults: new { controller = "Home", action = "FetchResult", id = UrlParameter.Optional }
         );

        }
    }
}
