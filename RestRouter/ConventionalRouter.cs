using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace RestRouter
{
	public class ConventionalRouter
	{
		private readonly HttpConfiguration _configuration;
		private readonly List<TypedRoute> _routes;

		public ConventionalRouter(HttpConfiguration configuration)
		{
			_configuration = configuration;
			_routes = new List<TypedRoute>();
		}

		public IEnumerable<TypedRoute> Routes  { get { return _routes; } }

		public void AddRoutes<TController>(List<IRouteConvetion> conventions) where TController : IHttpController
		{
			var type = typeof(TController);
			var methods = type
				.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
				.Where(m => m.ReturnType != typeof(void))
				.ToList();

			foreach (var method in methods)
			{
				var rt = new RouteTemplateBuilder(type, method);
				conventions.ForEach(convention => convention.Execute(rt));

				var route = new TypedRoute(rt.Build());
				route.Action(method.Name);
				route.Controller<TController>();

				_routes.Add(route);
				AddToConfiguration(_configuration, route);
			}
		}

		private void AddToConfiguration(HttpConfiguration config, TypedRoute route)
		{
			if (TypedDirectRouteProvider.Routes.ContainsKey(route.ControllerType))
			{
				var controllerLevelDictionary = TypedDirectRouteProvider.Routes[route.ControllerType];
				controllerLevelDictionary.Add(route.ActionName, route);
			}
			else
			{
				var controllerLevelDictionary = new Dictionary<string, TypedRoute> { { route.ActionName, route } };
				TypedDirectRouteProvider.Routes.Add(route.ControllerType, controllerLevelDictionary);
			}
		}
	}
}
