using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Conifer
{
	public class TypedDirectRouteProvider : DefaultDirectRouteProvider
	{
		private readonly Router _routes;

		public TypedDirectRouteProvider(Router routes)
		{
			_routes = routes;
		}

		protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
		{
			var controllerType = actionDescriptor.ControllerDescriptor.ControllerType;
			var actionName = actionDescriptor.ActionName;

			var routes = _routes
				.RoutesFor(controllerType, actionName)
				.ToList();

			actionDescriptor
				.SupportedHttpMethods
				.AddRange(routes.SelectMany(r => r.SupportedMethods).Distinct());

			return routes;
		}
	}
}
