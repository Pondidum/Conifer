using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RestRouter
{
	public class RouteBuilder
	{
		private readonly List<TypedRoute> _routes;

		public RouteBuilder(List<TypedRoute> routes)
		{
			_routes = routes;
		}

		public string RouteFor<T>(Expression<Action<T>> expression)
		{
			var info = MethodBuilder.GetMethodInfo(expression);

			var routes = _routes
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
