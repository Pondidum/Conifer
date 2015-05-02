using System.Web.Http;
using TypedRoutingTest.Controllers;

namespace TypedRoutingTest
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes(new TypedDirectRouteProvider());

			config.TypedRoute("", c => c.Action<HomeController>(h => h.Get()));
		}
	}
}
