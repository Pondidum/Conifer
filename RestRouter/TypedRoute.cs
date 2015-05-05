using System;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace RestRouter
{
	public class TypedRoute : IDirectRouteFactory
	{
		public string Template { get; private set; }
		public Type ControllerType { get; private set; }
		public string ActionName { get; private set; }

		public TypedRoute(string template)
		{
			Template = template;
		}

		RouteEntry IDirectRouteFactory.CreateRoute(DirectRouteFactoryContext context)
		{
			var builder = context.CreateBuilder(Template);

			return builder.Build();
		}

		public TypedRoute Controller<TController>() where TController : IHttpController
		{
			ControllerType = typeof(TController);
			return this;
		}

		public TypedRoute Action(string actionName)
		{
			ActionName = actionName;
			return this;
		}
	}
}
