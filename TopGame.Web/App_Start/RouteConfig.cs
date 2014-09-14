using System.Web.Mvc;
using System.Web.Routing;

namespace TopGame.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                "Base",
                "Jogos/{jogoName}/{jogoId}/{action}/{id}",
                new
                {
                    controller = "Jogatina", 
                    action = "Index",
                    jogoName = @"([a-zA-Z0-9]+-?)+",
                    jogoId = @"([a-z0-9])+", 
                    id = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Jogatina", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
