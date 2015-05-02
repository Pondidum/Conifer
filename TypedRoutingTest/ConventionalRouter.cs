using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TypedRoutingTest
{
	public class ConventionalRouter
	{
		private readonly HttpConfiguration _configuration;

		public ConventionalRouter(HttpConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void AddRoutes<TController>(string prefix)
			where TController : IHttpController
		{
			var type = typeof(TController);
			var methods = type
				.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
				.Where(m => m.ReturnType != typeof(void))
				.ToList();

			prefix = prefix.TrimEnd('/');

			foreach (var method in methods)
			{
				//make into a convention
				var parameterNames = method.GetParameters().Select(p => "{" + p.Name + "}");

				var raw = method.Name.EndsWith("raw", StringComparison.OrdinalIgnoreCase)
					? "raw"
					: "";

				var template = string.Format("{0}/{1}/{2}", prefix, string.Join("/", parameterNames), raw);

				var route = new TypedRoute(template);
				route.Action(method.Name);
				route.Controller<TController>();

				_configuration.TypedRoute(route);
			}
		} 
	}
}