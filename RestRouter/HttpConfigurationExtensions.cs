using System;
using System.Collections.Generic;
using System.Web.Http;

namespace RestRouter
{
	public static class HttpConfigurationExtensions
	{
		public static TypedRoute TypedRoute(this HttpConfiguration config, string template, Action<TypedRoute> configSetup)
		{
			var route = new TypedRoute(template);
			configSetup(route);

			return config.TypedRoute(route);
		}

		public static TypedRoute TypedRoute(this HttpConfiguration config, TypedRoute route)
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

			return route;
		}
	}
}
