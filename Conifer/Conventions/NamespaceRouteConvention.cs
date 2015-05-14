using System;
using System.Linq;

namespace Conifer.Conventions
{
	public class NamespaceRouteConvention : IRouteConvention
	{
		private bool _ignoreRootNamespace;
		private bool _ignoreControllersNamespace;
		private string _prefix;

		public NamespaceRouteConvention()
		{
			_ignoreRootNamespace = true;
			_ignoreControllersNamespace = true;
		}

		public NamespaceRouteConvention DontIgnoreRootNamespace()
		{
			_ignoreRootNamespace = false;
			return this;
		}

		public NamespaceRouteConvention DontIgnoreControllersNamespace()
		{
			_ignoreControllersNamespace = false;
			return this;
		}

		public NamespaceRouteConvention IgnoreThePrefix(string ns)
		{
			_prefix = ns.TrimEnd('.') + '.';
			_ignoreRootNamespace = false;
			return this;
		}

		public void Execute(TypedRouteBuilder template)
		{
			var controller = template.Controller;
			var ns = controller.Namespace;

			if (string.IsNullOrWhiteSpace(ns))
			{
				return;
			}

			if (string.IsNullOrWhiteSpace(_prefix) == false)
			{
				if (ns.StartsWith(_prefix, StringComparison.OrdinalIgnoreCase))
				{
					ns = ns.Substring(_prefix.Length);
				}
			}

			var segments = ns.Split('.').AsEnumerable();

			if (_ignoreRootNamespace)
			{
				segments = segments
					.Skip(1);
			}

			if (_ignoreControllersNamespace)
			{
				segments = segments
					.Where(s => s.Equals("controllers", StringComparison.OrdinalIgnoreCase) == false);
			}

			template.Parts.AddRange(segments);
		}
	}
}
