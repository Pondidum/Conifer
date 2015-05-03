using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TypedRoutingTest
{
	public class ConventionalRouter
	{
		private readonly HttpConfiguration _configuration;

		public ConventionalRouter(HttpConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void AddRoutes<TController>(List<IRouteConvetion> conventions) where TController : IHttpController
		{
			var type = typeof(TController);
			var methods = type
				.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
				.Where(m => m.ReturnType != typeof(void))
				.ToList();

			foreach (var method in methods)
			{
				var rt = new RouteTemplateBuilder(method);
				conventions.ForEach(convention => convention.Execute(rt));

				var route = new TypedRoute(rt.Build());
				route.Action(method.Name);
				route.Controller<TController>();

				_configuration.TypedRoute(route);
			}
		}
	}
}
