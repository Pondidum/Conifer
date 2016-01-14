using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Conifer
{
	public class TypedRoute : IDirectRouteFactory
	{
		public string Template { get; private set; }
		public HashSet<HttpMethod> SupportedMethods { get; private set; }
		public Type ControllerType { get; private set; }
		public MethodInfo Method { get; private set; }
		public string ActionName { get; private set; }

		public TypedRoute(string template, HashSet<HttpMethod> supportedMethods, Type controller, MethodInfo method)
		{
			if (typeof(IHttpController).IsAssignableFrom(controller) == false)
			{
				throw new ArgumentException(
					string.Format("{0} does not implement IHttpController.", controller),
					"controller");
			}

			Template = template;
			SupportedMethods = supportedMethods;
			ControllerType = controller;
			Method = method;
			ActionName = method.Name;
		}

		RouteEntry IDirectRouteFactory.CreateRoute(DirectRouteFactoryContext context)
		{
			var builder = context.CreateBuilder(Template);

			return builder.Build();
		}

		public override string ToString()
		{
			return string.Format(
				"[{0}] {1}",
				string.Join(" ", SupportedMethods.Select(m => m.Method).OrderBy(m => m)),
				Template);
		}
	}
}
