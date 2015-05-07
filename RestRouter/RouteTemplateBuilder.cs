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

		public RouteTemplateBuilder(Type controller, MethodInfo method)
		{
			Controller = controller;
			Method = method;
			Parts = new List<string>();
		}

		public string Build()
		{
			return string.Join("/", Parts.Select(p => p.Trim('/')));
		}
	}
}