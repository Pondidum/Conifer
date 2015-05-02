using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using TypedRoutingTest.Controllers;

namespace TypedRoutingTest
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes(new TypedDirectRouteProvider());

			config.TypedRoute("", c => c.Action<HomeController>(h => h.Get()));

			var conventional = new ConventionalRouter(config);

			conventional.AddRoutes<CandidateController>("candidate/ref");

		}
	}
}
