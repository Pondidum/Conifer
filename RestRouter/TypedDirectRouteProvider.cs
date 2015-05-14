using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace RestRouter
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

			var routes = _routes.RoutesFor(controllerType, actionName, parameters);

			return routes.ToList();
		}
	}
}
