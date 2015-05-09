using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace RestRouter
{
	public class TypedDirectRouteProvider : DefaultDirectRouteProvider
	{
		private readonly Dictionary<Type, Dictionary<string, TypedRoute>> _routes;

		public TypedDirectRouteProvider()
		{
			 _routes = new Dictionary<Type, Dictionary<string, TypedRoute>>();
		}

		public void AddRoute(TypedRoute route)
		{
			if (_routes.ContainsKey(route.ControllerType))
			{
				_routes[route.ControllerType].Add(route.ActionName, route);
			}
			else
			{
				var controllerLevelDictionary = new Dictionary<string, TypedRoute> { { route.ActionName, route } };
				_routes.Add(route.ControllerType, controllerLevelDictionary);
			}
		}

		protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
		{
			var factories = base.GetActionRouteFactories(actionDescriptor).ToList();

			if (_routes.ContainsKey(actionDescriptor.ControllerDescriptor.ControllerType))
			{
				var controllerLevelDictionary = _routes[actionDescriptor.ControllerDescriptor.ControllerType];
				if (controllerLevelDictionary.ContainsKey(actionDescriptor.ActionName))
				{
					factories.Add(controllerLevelDictionary[actionDescriptor.ActionName]);
				}
			}
 
			return factories;
		}
	}
}
