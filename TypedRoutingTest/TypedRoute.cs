using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace TypedRoutingTest
{
	public class TypedRoute : IDirectRouteFactory
	{
		private readonly string _template;

		public TypedRoute(string template)
		{
			_template = template;
		}

		public Type ControllerType { get; private set; }
		public string ActionName { get; private set; }


		RouteEntry IDirectRouteFactory.CreateRoute(DirectRouteFactoryContext context)
		{
			var builder = context.CreateBuilder(_template);

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

		//private static MethodInfo GetMethodInfoInternal(dynamic expression)
		//{
		//	var method = expression.Body as MethodCallExpression;
		//	if (method != null)
		//		return method.Method;

		//	throw new ArgumentException("Expression is incorrect!");
		//}
	}
}
