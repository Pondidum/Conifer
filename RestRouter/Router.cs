using System;
using System.Web.Http;

namespace RestRouter
{
	public class Router
	{
		public static RouteBuilder Create(HttpConfiguration httpConfiguration, Action<RouterConfigurationExpression> configure)
		{
			var routeProvider = new TypedDirectRouteProvider();
			var router = new ConventionalRouter(routeProvider);
			var expression = new RouterConfigurationExpression(router);
			configure(expression);

			httpConfiguration.MapHttpAttributeRoutes(routeProvider);

			return new RouteBuilder(router.Routes);
		} 
	}
}