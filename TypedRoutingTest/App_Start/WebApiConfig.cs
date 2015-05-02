using System;
using System.Collections.Generic;
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

			var conventions = new List<IRouteConvetion>
			{
				new PrefixRouteConvention("candidate/ref"),
				new ParameterNameRouteConvention(),
				new RawOptionRouteConvention()
			};

			conventional.AddRoutes<CandidateController>(conventions);

		}
	}
}
