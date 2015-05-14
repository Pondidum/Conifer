using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Conifer
{
	public class TypedRouteBuilder
	{
		public Type Controller { get; private set; }
		public MethodInfo Method { get; private set; }
		public List<string> Parts { get; private set; }

		public TypedRouteBuilder(Type controller, MethodInfo method)
		{
			Controller = controller;
			Method = method;
			Parts = new List<string>();
		}

		public TypedRoute Build(List<IRouteConvention> conventions )
		{
			conventions.ForEach(convention => convention.Execute(this));

			var template = string.Join("/", Parts.Select(p => p.Trim('/')));

			return new TypedRoute(template, Controller, Method.Name);
		}
	}
}
