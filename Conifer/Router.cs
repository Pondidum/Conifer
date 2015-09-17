using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;

namespace Conifer
{
	public class Router
	{
		private readonly List<TypedRoute> _routes;

		public Router(HttpConfiguration http, RouterConfigurationExpression expression)
		{
			var router = new ConventionalRouter();
			expression.ApplyTo(router);

			_routes = router.Routes.ToList();

			http.MapHttpAttributeRoutes(new TypedDirectRouteProvider(this));
		}


		public Router(HttpConfiguration http, Action<RouterConfigurationExpression> configure)
			:this(http, CreateConfigurations(configure))
		{
		}

		private static RouterConfigurationExpression CreateConfigurations(Action<RouterConfigurationExpression> configure)
		{
			var expression = new RouterConfigurationExpression();
			configure(expression);
			return expression;
		}

		public string LinkTo<T>(Expression<Action<T>> expression)
		{
			var info = MethodBuilder.GetMethodInfo(expression);
			var routes = RoutesFor(info.Class, info.Method.Name, info.Parameters.Keys);
			var template = routes.First().Template;

			foreach (var pair in info.Parameters)
			{
				template = template.Replace("{" + pair.Key + "}", Convert.ToString(pair.Value));
			}

			return template;
		}

		public IEnumerable<TypedRoute> RoutesFor(Type controllerType, string actionName)
		{
			return _routes
				.Where(r => r.ControllerType == controllerType)
				.Where(r => r.ActionName == actionName);
		}

		public IEnumerable<TypedRoute> RoutesFor(Type controllerType, string actionName, IEnumerable<string> parameterNames)
		{
			return RoutesFor(controllerType, actionName)
				.Where(r => parameterNames.All(p => r.Template.Contains("{" + p + "}")));
		}

		public IEnumerable<TypedRoute> AllRoutes()
		{
			return _routes;
		}
	}
}
