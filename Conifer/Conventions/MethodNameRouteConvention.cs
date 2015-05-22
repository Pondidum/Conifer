using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace Conifer.Conventions
{
	public class MethodNameRouteConvention : IRouteConvention
	{
		private bool _removePrefixes;
		private List<HttpMethod> _customPrefixes;

		private readonly Lazy<IEnumerable<HttpMethod>> _prefixes;

		public MethodNameRouteConvention()
		{
			_removePrefixes = true;
			_prefixes = new Lazy<IEnumerable<HttpMethod>>(() =>
			{
				if (_customPrefixes != null && _customPrefixes.Any())
				{
					return _customPrefixes;
				}

				return typeof(HttpMethod)
					.GetProperties(BindingFlags.Static | BindingFlags.Public)
					.Select(p => p.GetGetMethod())
					.Select(m => m.Invoke(null, new object[] { }))
					.Cast<HttpMethod>();

			});
		}

		public void Execute(TypedRouteBuilder template)
		{
			var name = template.Method.Name;

			if (_removePrefixes)
			{
				var prefix = _prefixes
					.Value
					.FirstOrDefault(p => name.StartsWith(p.Method, StringComparison.OrdinalIgnoreCase));

				if (prefix != null)
				{
					name = name.Substring(prefix.Method.Length);

					if (template.SupportedMethods.Contains(prefix) == false)
					{
						template.SupportedMethods.Add(prefix);
					}
				}
			}

			if (string.IsNullOrWhiteSpace(name))
			{
				return;
			}

			template.Parts.Add(new ActionRoutePart { Value = name });
		}

		public MethodNameRouteConvention DontStripVerbPrefixes()
		{
			_removePrefixes = false;
			return this;
		}

		public MethodNameRouteConvention UseCustomPrefixes(IEnumerable<HttpMethod> prefixes)
		{
			_customPrefixes = prefixes.ToList();
			return this;
		}
	}
}
