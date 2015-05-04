using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TypedRoutingTest
{
	public class ConventionalRouter
	{
		private readonly HttpConfiguration _configuration;
		private readonly List<TypedRoute> _allRoutes;

		public ConventionalRouter(HttpConfiguration configuration)
		{
			_configuration = configuration;
			_allRoutes = new List<TypedRoute>();
		}

		public void AddRoutes<TController>(List<IRouteConvetion> conventions) where TController : IHttpController
		{
			var type = typeof(TController);
			var methods = type
				.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
				.Where(m => m.ReturnType != typeof(void))
				.ToList();

			foreach (var method in methods)
			{
				var rt = new RouteTemplateBuilder(method);
				conventions.ForEach(convention => convention.Execute(rt));

				var route = new TypedRoute(rt.Build());
				route.Action(method.Name);
				route.Controller<TController>();

				_allRoutes.Add(route);
				_configuration.TypedRoute(route);
			}
		}

		public string LinkTo<T>(Expression<Action<T>> expression)
		{
			var info = GetMethodInfo(expression);

			var routes = _allRoutes
				.Where(r => r.ControllerType == info.Class)
				.Where(r => r.ActionName == info.Method.Name);

			var template = routes.First().Template;

			foreach (var pair in info.Parameters)
			{
				template = template.Replace("{" + pair.Key + "}", Convert.ToString(pair.Value));
			}

			return template;
		}

		private class MethodModel
		{
			public Type Class { get; set; }
			public MethodInfo Method { get; set; }
			public Dictionary<string, object> Parameters { get; set; }
		}

		private static MethodModel GetMethodInfoInternal(dynamic expression)
		{
			var method = expression.Body as MethodCallExpression;
			if (method == null) throw new ArgumentException("Expression is incorrect!");

			var parameters = method.Method.GetParameters();
			var values = method.Arguments.Select(a => Expression.Lambda(a).Compile().DynamicInvoke()).ToList();

			return new MethodModel
			{
				Method = method.Method,
				Parameters = parameters
					.Select((p, i) => new
					{
						Name = p.Name,
						Value = values[i]
					})
					.ToDictionary(p => p.Name, p => p.Value)
			};
		}

		private MethodModel GetMethodInfo<T>(Expression<Action<T>> expression)
		{
			var info = GetMethodInfoInternal(expression);
			info.Class = typeof(T);

			return info;
		}
	}
}
