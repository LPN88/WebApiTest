using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;

namespace WebApiTest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",               
                defaults: new { controller = "Values", action = "GetTest", id = RouteParameter.Optional }
            );
            //var container = new UnityContainer();
            //container.RegisterType<IMetricsR3, MetricsR3>();
            //config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
