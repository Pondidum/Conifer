using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Conifer
{
	public class RouteBuilder
	{
		private readonly List<TypedRoute> _routes;

		public RouteBuilder(IEnumerable<TypedRoute> routes)
		{
			_routes = routes.ToList();
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

		public IEnumerable<TypedRoute> RoutesFor(Type controllerType, string actionName, IEnumerable<string> parameterNames)
		{
			return _routes
				.Where(r => r.ControllerType == controllerType)
				.Where(r => r.ActionName == actionName)
				.Where(r => parameterNames.All(p => r.Template.Contains("{" + p + "}")));
		}
	}
}
