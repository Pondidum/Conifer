using System;
using System.Web.Http;

namespace Conifer
{
	public class Router
	{
		public static RouteBuilder Create(HttpConfiguration httpConfiguration, Action<RouterConfigurationExpression> configure)
		{
			var router = new ConventionalRouter();
			var expression = new RouterConfigurationExpression(router);
			configure(expression);

			var routeBuilder = new RouteBuilder(router.Routes);

			httpConfiguration.MapHttpAttributeRoutes(new TypedDirectRouteProvider(routeBuilder));

			return routeBuilder;
		}
	}
}