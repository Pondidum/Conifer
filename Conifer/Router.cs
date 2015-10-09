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

		/// <summary>
		/// Creates a Router using a pre-configured RouterConfigurationExpression.
		/// </summary>
		/// <param name="http">The HttpConfiguration which will handle the routes.</param>
		/// <param name="expression">The fully configured RouterConfiguration.  Changes after this method call will have no effect on the Router.</param>
		public Router(HttpConfiguration http, RouterConfigurationExpression expression)
		{
			var router = new ConventionalRouter();
			expression.ApplyTo(router);

			_routes = router.Routes.ToList();

			http.MapHttpAttributeRoutes(new TypedDirectRouteProvider(this));
		}

		/// <summary>
		/// Creates a Router using a configuration lambda.
		/// </summary>
		/// <param name="http">The HttpConfiguration which will handle the routes.</param>
		/// <param name="configure">The configuration expression.</param>
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

		/// <summary>
		/// Creates a url for the given controller + action, substituting in argument values specified.
		/// </summary>
		/// <typeparam name="TController">The controller to link to.</typeparam>
		/// <param name="expression">The action, e.g. <code>c =&gt; c.GetByID(1234);</code></param>
		/// <returns>The url, e.g. <code>Books/ByID/1234</code></returns>
		public string LinkTo<TController>(Expression<Action<TController>> expression)
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

		/// <summary>
		/// Gets the template for the given controller and action, without replacing values specified.
		/// </summary>
		/// <typeparam name="TController">The controller to link to.</typeparam>
		/// <param name="expression">The action, e.g. <code>c =&gt; c.GetByID(1234);</code></param>
		/// <returns>The url, e.g. <code>Books/ByID/{id}</code></returns>
		public string TemplateFor<TController>(Expression<Action<TController>> expression)
		{
			var info = MethodBuilder.GetMethodInfo(expression);
			var routes = RoutesFor(info.Class, info.Method.Name, info.Parameters.Keys);

			return routes.First().Template;
		}

		/// <summary>
		/// Gets all the possible routes for a given controller and action.
		/// </summary>
		public IEnumerable<TypedRoute> RoutesFor(Type controllerType, string actionName)
		{
			return _routes
				.Where(r => r.ControllerType == controllerType)
				.Where(r => r.ActionName == actionName);
		}

		/// <summary>
		/// Gets all the possible routes for a given controller, action, and matching parameters.
		/// </summary>
		public IEnumerable<TypedRoute> RoutesFor(Type controllerType, string actionName, IEnumerable<string> parameterNames)
		{
			return RoutesFor(controllerType, actionName)
				.Where(r => parameterNames.All(p => r.Template.Contains("{" + p + "}")));
		}

		/// <summary>
		/// Gets all routes registered in the Router.
		/// </summary>
		public IEnumerable<TypedRoute> AllRoutes()
		{
			return _routes;
		}
	}
}
