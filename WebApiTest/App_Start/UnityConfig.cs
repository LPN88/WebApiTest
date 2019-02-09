using System.Web.Http;
using Unity;

namespace WebApiTest
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IMetricsR3, MetricsR3>();
            //GlobalConfiguration.Configuration.DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new UnityResolver(container);
            //var container = new UnityContainer();
            //container.RegisterType<IRepository, Repository>();
            //config.DependencyResolver = new UnityResolver(container);
        }
    }
}