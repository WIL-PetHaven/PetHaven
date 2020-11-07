using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PetHaven
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "AnimalsCreate",
                url: "Animals/Create",
                defaults: new { controller = "Animals", action = "Create" }
            );

            routes.MapRoute(
                name: "AnimalsbyCategorybyPage",
                url: "Animals/{category}/Page{page}",
                defaults: new { controller = "Animals", action = "Index" }
            );

            routes.MapRoute(
                name: "AnimalsbyPage",
                url: "Animals/Page{page}",
                defaults: new
                { controller = "Animals", action = "Index" }
            );

            routes.MapRoute(
                name: "AnimalsbyCategory",
                url: "Animals/{category}",
                defaults: new { controller = "Animals", action = "Index" }
            );

            routes.MapRoute(
                name: "AnimalsIndex",
                url: "Animals",
                defaults: new { controller = "Animals", action = "Index" }
            );
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
