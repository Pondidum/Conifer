using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using RestRouter;
using RestRouter.Conventions;
using TypedRoutingTest.Controllers;

namespace TypedRoutingTest
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			var conventions = new List<IRouteConvention>
			{
				new ControllerNameRouteConvention(),
				new SpecifiedPartRouteConvention("ref"),
				new ParameterNameRouteConvention(),
				new ActionEndsWithRawRouteConvention()
			};

			config.AddConventionalRoutes(router =>
			{
				router.DefaultConventionsAre(conventions);
				router.Add<CandidateController>(); //uses default conventions
				router.Add<HomeController>(Enumerable.Empty<IRouteConvention>()); //uses none
			});
		}
	}
}
