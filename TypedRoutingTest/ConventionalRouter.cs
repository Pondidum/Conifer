using System;
using System.Collections.Generic;
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

			var conventions = new List<Action<RouteTemplate>>
			{
				rt =>
				{
					rt.Parts.Insert(0, prefix.TrimEnd('/'));
				}, 
				rt =>
				{
					var parameterNames = rt.Method.GetParameters().Select(p => "{" + p.Name + "}");
					rt.Parts.Add(string.Join("/", parameterNames));
				},
				rt =>
				{
					if (rt.Method.Name.EndsWith("Raw"))
					{
						rt.Parts.Add("raw");
					}
				}
			};

			var type = typeof(TController);
			var methods = type
				.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
				.Where(m => m.ReturnType != typeof(void))
				.ToList();

			foreach (var method in methods)
			{
				var rt = new RouteTemplate(method);
				conventions.ForEach(convention => convention(rt));

				var template = string.Join("/", rt.Parts.Select(p => p.Trim('/')));

				var route = new TypedRoute(template);
				route.Action(method.Name);
				route.Controller<TController>();

				_configuration.TypedRoute(route);
			}
		}

		private class RouteTemplate
		{
			public MethodInfo Method { get; private set; }
			public List<string> Parts { get; private set; }

			public RouteTemplate(MethodInfo method)
			{
				Method = method;
				Parts = new List<string>();
			}
		}
	}
}