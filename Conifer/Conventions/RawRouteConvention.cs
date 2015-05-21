using System;
using System.Linq;

namespace Conifer.Conventions
{
	public class RawRouteConvention : IRouteConvention
	{
		public void Execute(TypedRouteBuilder template)
		{
			if (template.Method.Name.EndsWith("Raw"))
			{
				template.Parts.Add(new RoutePart(PartType.Constant) { Value = "raw"});
			}

			var action = template
				.Parts
				.FirstOrDefault(p => p.Type == PartType.Action);

			if (action == null)
			{
				return;
			}

			if (action.Value.EndsWith("raw", StringComparison.OrdinalIgnoreCase))
			{
				action.Value = action.Value.Substring(0, action.Value.Length - 3);
			}
		}
	}
}