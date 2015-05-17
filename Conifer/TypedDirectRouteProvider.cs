using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Conifer
{
	public class TypedDirectRouteProvider : DefaultDirectRouteProvider
	{
		private readonly RouteBuilder _routes;

		public TypedDirectRouteProvider(RouteBuilder routes)
		{
			_routes = routes;
		}

		protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
		{
			var controllerType = actionDescriptor.ControllerDescriptor.ControllerType;
			var actionName = actionDescriptor.ActionName;
			var parameters = actionDescriptor.GetParameters().Select(p => p.ParameterName);

			var routes = _routes
				.RoutesFor(controllerType, actionName, parameters)
				.ToList();

			actionDescriptor
				.SupportedHttpMethods
				.AddRange(routes.SelectMany(r => r.SupportedMethods).Distinct());

			return routes;
		}
	}
}
