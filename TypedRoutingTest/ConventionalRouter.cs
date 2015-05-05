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
			var info = MethodBuilder.GetMethodInfo(expression);

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
	}
}
