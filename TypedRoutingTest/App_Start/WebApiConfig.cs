using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using RestRouter;
using RestRouter.Conventions;
using TypedRoutingTest.Controllers;

namespace TypedRoutingTest
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes(new TypedDirectRouteProvider());

			var conventional = new ConventionalRouter(config);
			var conventions = new List<IRouteConvetion>
			{
				new ControllerNameRouteConvention(),
				new SpecifiedPartRouteConvention("ref"),
				new ParameterNameRouteConvention(),
				new RawOptionRouteConvention()
			};

			conventional.AddRoutes<CandidateController>(conventions);

			conventional.AddRoutes<HomeController>(Enumerable.Empty<IRouteConvetion>().ToList());
		}
	}
}
