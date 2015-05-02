using System.Collections.Generic;
using System.Reflection;

namespace TypedRoutingTest
{
	public class RouteTemplate
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
