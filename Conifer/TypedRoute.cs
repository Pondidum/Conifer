using System;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Conifer
{
	public class TypedRoute : IDirectRouteFactory
	{
		public string Template { get; private set; }
		public Type ControllerType { get; private set; }
		public string ActionName { get; private set; }

		public TypedRoute(string template, Type controller, string actionName)
		{
			if (typeof(IHttpController).IsAssignableFrom(controller) == false)
			{
				throw new ArgumentException(
					string.Format("{0} does not implement IHttpController.", controller),
					"controller");
			}

			Template = template;
			ControllerType = controller;
			ActionName = actionName;
		}

		RouteEntry IDirectRouteFactory.CreateRoute(DirectRouteFactoryContext context)
		{
			var builder = context.CreateBuilder(Template);

			return builder.Build();
		}
	}
}
