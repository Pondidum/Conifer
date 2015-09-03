using System.Web.Http;
using Conifer;
using Controllers;
using StructureMap;
using StructureMap.Graph;

namespace AppStart
{
	public class ApiStart
	{
		public static void Register(HttpConfiguration config)
		{
			var router = RouterCreator.Create(config, r =>
			{
				r.AddAll<RootController>(null);
				r.AddAll<BooksController>();
			});

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
