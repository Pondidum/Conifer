using System;
using System.Collections.Generic;
using System.Reflection;

namespace RestRouter
{
	public class MethodModel
	{
		public Type Class { get; set; }
		public MethodInfo Method { get; set; }
		public Dictionary<string, object> Parameters { get; set; }
	}
}
