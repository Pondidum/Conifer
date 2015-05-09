using System;

namespace RestRouter.Conventions
{
	public class ControllerNameRouteConvention : IRouteConvention
	{
		public void Execute(RouteTemplateBuilder template)
		{
			var name = template.Controller.Name;

			if (name.Equals("controller", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}

			if (name.EndsWith("controller", StringComparison.OrdinalIgnoreCase))
			{
				name = name.Substring(0, name.LastIndexOf("controller", StringComparison.OrdinalIgnoreCase));
			}

			template.Parts.Add(name);
		}
	}
}
