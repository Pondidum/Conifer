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
			var router = Router.Create(config, r =>
			{
				r.Add<RootController>(null);
				r.Add<BooksController>();
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
