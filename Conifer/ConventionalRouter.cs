using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Http.Controllers;

namespace Conifer
{
	public class ConventionalRouter : IConventionalRouter
	{
		private readonly List<TypedRoute> _routes;

		public ConventionalRouter()
		{
			_routes = new List<TypedRoute>();
		}

		public IEnumerable<TypedRoute> Routes { get { return _routes; } }

		public void AddRoutes<TController>(List<IRouteConvention> conventions) where TController : IHttpController
		{
			AddRoutes(typeof(TController), conventions);
		}

		public void AddRoutes(Type controllerType, List<IRouteConvention> conventions)
		{
			var httpController = typeof(IHttpController);

			if (httpController.IsAssignableFrom(controllerType) == false)
			{
				throw new ArgumentException(
					string.Format("{0} must implement {1}", controllerType.Name, httpController.Name),
					"controllerType");
			}

			var methods = FindMethods(controllerType);

			foreach (var template in methods)
			{
				var route = template.Build(conventions);

				_routes.Add(route);
			}
		}

		public void AddRoute<TController>(Expression<Action<TController>> expression, List<IRouteConvention> conventions)
			where TController : IHttpController
		{
			var method = MethodBuilder.GetMethodInfo(expression).Method;

			if (IsValidAction(method) == false)
			{
				throw new ArgumentException(expression + " is not a valid controller action", "expression");
			}

			var template = new TypedRouteBuilder(typeof(TController), method);

			_routes.Add(template.Build(conventions));
		}

		private static IEnumerable<TypedRouteBuilder> FindMethods(Type controllerType)
		{
			return controllerType
				.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
				.Where(IsValidAction)
				.Select(m => new TypedRouteBuilder(controllerType, m));
		}

		private static bool IsValidAction(MethodInfo method)
		{
			return method.IsPublic && method.IsStatic == false && method.ReturnType != typeof(void);
		}
	}
}
