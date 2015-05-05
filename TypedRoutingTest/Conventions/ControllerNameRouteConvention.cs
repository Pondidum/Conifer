using System;
using RestRouter;

namespace TypedRoutingTest.Conventions
{
	public class ControllerNameRouteConvention : IRouteConvetion
	{
		public void Execute(RouteTemplateBuilder template)
		{
			var name = template.Controller.Name;

			if (name.EndsWith("controller", StringComparison.OrdinalIgnoreCase))
			{
				name = name.Substring(0, name.LastIndexOf("Controller", StringComparison.OrdinalIgnoreCase));
			}

			template.Parts.Add(name);
		}
	}
}
