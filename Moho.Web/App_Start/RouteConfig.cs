using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moho.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ShowScreen",
                url: "{uriName}",
                defaults: new { controller = "Screen", action = "Show" }
            );

            routes.MapRoute(
                name: "AddScreenItem",
                url: "{uriName}/add",
                defaults: new { controller = "Screen", action = "Create" }
            );

            routes.MapRoute(
                name: "EditScreenItem",
                url: "{uriName}/edit/{id}",
                defaults: new { controller = "Screen", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DelScreenItem",
                url: "{uriName}/del/{id}",
                defaults: new { controller = "Screen", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
