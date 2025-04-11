using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Book_Store
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Route for the Administrator Interface
            routes.MapRoute(
                name: "AdministratorInterface",
                url: "Book/Administrator_Interface",
                defaults: new { controller = "Book", action = "Administrator" }
            );

           
            // Default Route (Handles Other Controllers)
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}