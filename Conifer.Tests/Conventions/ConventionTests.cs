using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Conifer.Tests.Conventions
{
	public class ConventionTests
	{
		public string Route { get; private set; }
		public HashSet<HttpMethod> SupportedMethods { get; private set; }

		public Func<IRouteConvention> Convention { get; set; }
		public Action<TypedRouteBuilder> PreConfigure { get; set; }

		public ConventionTests()
		{
			PreConfigure = t => { };
		}
 
		protected void ExecuteConventionOn<T>() where T : IHttpController
		{
			ExecuteConventionOn<T>(c => c.ToString());
		}

		protected void ExecuteConventionOn<T>(Expression<Action<T>> expression) where T : IHttpController
		{
			var method = (expression.Body as MethodCallExpression).Method;
			var template = new TypedRouteBuilder(typeof(T), method);
			
			PreConfigure.Invoke(template);

			var convention = Convention.Invoke();
			convention.Execute(template);

			SupportedMethods = template.SupportedMethods;
			Route = template
				.Parts
				.Select(p => p.Value)
				.Aggregate("", (a, v) => a + "/" + v);
		}
	}
}
