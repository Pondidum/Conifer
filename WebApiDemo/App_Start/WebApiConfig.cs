using System.Web.Http;
using Conifer;
using Conifer.Conventions;
using StructureMap;
using StructureMap.Graph;
using WebApiDemo.Controllers;

namespace WebApiDemo
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			var router = RouterCreator.Create(config, r =>
			{
				//setup the default conventions, these 3 will generate routes like:
				//  /Person/View/1234
				r.DefaultConventionsAre(new IRouteConvention[]
				{
					new ControllerNameRouteConvention(),
					new MethodNameRouteConvention(),
					new ParameterNameRouteConvention(),
					new RawRouteConvention()
				});

				r.AddAll<HomeController>(null);	//no conventions applied to this route
				r.AddAll<PersonController>();
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
				c.For<Router>().Use(router);
			}));
		}
	}
}
