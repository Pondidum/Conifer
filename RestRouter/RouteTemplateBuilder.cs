using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RestRouter
{
	public class RouteTemplateBuilder
	{
		public Type Controller { get; private set; }
		public MethodInfo Method { get; private set; }
		public List<string> Parts { get; private set; }

		public RouteTemplateBuilder(MethodInfo method)
		{
			Controller = method.DeclaringType;
			Method = method;
			Parts = new List<string>();
		}

		public TypedRoute Build()
		{
			var template = string.Join("/", Parts.Select(p => p.Trim('/')));

			return new TypedRoute(template, Controller, Method.Name);
		}
	}
}
