using System;
using System.Linq;

namespace RestRouter.Conventions
{
	public class NamespaceRouteConvention : IRouteConvention
	{
		public bool IgnoreRootNamespace { get; set; }
		public bool IgnoreControllersNamespace { get; set; }

		public NamespaceRouteConvention()
		{
			IgnoreRootNamespace = true;
			IgnoreControllersNamespace = true;
		}

		public void Execute(TypedRouteBuilder template)
		{
			var controller = template.Controller;
			var ns = controller.Namespace;

			if (string.IsNullOrWhiteSpace(ns))
			{
				return;
			}

			var segments = ns.Split('.').AsEnumerable();

			if (IgnoreRootNamespace)
			{
				segments = segments
					.Skip(1);
			}

			if (IgnoreControllersNamespace)
			{
				segments = segments
					.Where(s => s.Equals("controllers", StringComparison.OrdinalIgnoreCase) == false);
			}

			template.Parts.AddRange(segments);
		}
	}
}
