using System.Web.Http;
using RestRouter;
using RestRouter.Conventions;
using StructureMap;
using StructureMap.Graph;
using Test.WebApi.Controllers;

namespace Test.WebApi
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			var router = Router.Create(config, r =>
			{
				//setup the default conventions, these 3 will generate routes like:
				//  /Person/View/1234
				r.DefaultConventionsAre(new IRouteConvention[]
				{
					new ControllerNameRouteConvention(),
					new ParameterNameRouteConvention(),
					new ActionEndsWithRawRouteConvention()
				});

				r.Add<HomeController>(null);	//no conventions applied to this route
				r.Add<PersonController>();
			});

			//configure your container of choice
			config.DependencyResolver = new StructureMapDependencyResolver(new Container(c =>
			{
				c.Scan(a =>
				{
					a.TheCallingAssembly();
					a.WithDefaultConventions();
				});

				//just make sure RouteBuilder is the same instance each time:
				c.For<RouteBuilder>().Use(router);
			}));
		}
	}
}
