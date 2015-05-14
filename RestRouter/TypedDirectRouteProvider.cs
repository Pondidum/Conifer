using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace RestRouter
{
	public class TypedDirectRouteProvider : DefaultDirectRouteProvider
	{
		private readonly Dictionary<Type, List<TypedRoute>> _routes;

		public TypedDirectRouteProvider()
		{
			_routes = new Dictionary<Type, List<TypedRoute>>();
		}

		public void AddRoute(TypedRoute route)
		{
			if (_routes.ContainsKey(route.ControllerType))
			{
				_routes[route.ControllerType].Add(route);
			}
			else
			{
				_routes.Add(route.ControllerType, new List<TypedRoute> { route });
			}
		}

		protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
		{
			var factories = base.GetActionRouteFactories(actionDescriptor).ToList();

			if (_routes.ContainsKey(actionDescriptor.ControllerDescriptor.ControllerType))
			{
				var controllerRoutes = _routes[actionDescriptor.ControllerDescriptor.ControllerType];
				var parameters = actionDescriptor.GetParameters();

				factories.AddRange(controllerRoutes
					.Where(cr => cr.ActionName.Equals(actionDescriptor.ActionName, StringComparison.OrdinalIgnoreCase))
					.Where(cr => parameters.All(p => cr.Template.Contains("{" + p.ParameterName + "}"))));
			}

			return factories;
		}

		private static string GetName()
		{
			return string.Empty;
		}
	}
}
