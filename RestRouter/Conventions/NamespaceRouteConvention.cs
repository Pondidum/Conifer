using System.Linq;

namespace RestRouter.Conventions
{
	public class NamespaceRouteConvention : IRouteConvention
	{
		private readonly bool _ignoreRootNamespace;

		public NamespaceRouteConvention()
			: this(true)
		{
		}

		public NamespaceRouteConvention(bool ignoreRootNamespace)
		{
			_ignoreRootNamespace = ignoreRootNamespace;
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

			if (_ignoreRootNamespace)
			{
				segments = segments.Skip(1);
			}

			template.Parts.AddRange(segments);
		}
	}
}
