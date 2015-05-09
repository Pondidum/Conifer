using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;

namespace RestRouter
{
	public class ConventionalRouter
	{
		private readonly TypedDirectRouteProvider _routeProvider;
		private readonly List<TypedRoute> _routes;

		public ConventionalRouter(TypedDirectRouteProvider routeProvider)
		{
			_routeProvider = routeProvider;
			_routes = new List<TypedRoute>();
		}

		public IEnumerable<TypedRoute> Routes  { get { return _routes; } }

		public void AddRoutes<TController>(List<IRouteConvention> conventions) where TController : IHttpController
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
				_routeProvider.AddRoute(route);
			}
		}
	}
}
