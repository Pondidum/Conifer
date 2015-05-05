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
		public List<TypedRoute> AllRoutes { get; private set; }

		private readonly HttpConfiguration _configuration;

		public ConventionalRouter(HttpConfiguration configuration)
		{
			_configuration = configuration;
			AllRoutes = new List<TypedRoute>();
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
				var rt = new RouteTemplateBuilder(type, method);
				conventions.ForEach(convention => convention.Execute(rt));

				var route = new TypedRoute(rt.Build());
				route.Action(method.Name);
				route.Controller<TController>();

				AllRoutes.Add(route);
				_configuration.TypedRoute(route);
			}
		}
	}
}
