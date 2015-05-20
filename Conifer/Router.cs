using System;
using System.Web.Http;

namespace Conifer
{
	public class Router
	{
		/// <summary>
		/// Creates a strong-typed router for routing and linking actions
		/// </summary>
		/// <param name="httpConfiguration">The HttpConfiguration to attach the routes to.</param>
		/// <param name="configure">Expression to register controllers and setup conventions</param>
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