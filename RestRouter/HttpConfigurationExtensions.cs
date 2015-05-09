using System;
using System.Web.Http;

namespace RestRouter
{
	public static class HttpConfigurationExtensions
	{
		public static void AddConventionalRoutes(this HttpConfiguration self, Action<RouterConfigurationExpression> configure)
		{
			var router = new TypedDirectRouteProvider();
			var expression = new RouterConfigurationExpression(router);
			configure(expression);

			self.MapHttpAttributeRoutes(router);
		}
	}
}
