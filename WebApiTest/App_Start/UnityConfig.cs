using System.Web.Http;
using Unity;

namespace WebApiTest
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IMetricsR3, MetricsR3>();           
            GlobalConfiguration.Configuration.DependencyResolver = new UnityResolver(container);
        }
    }
}