using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Conifer.Conventions
{
	public class MethodNameRouteConvention : IRouteConvention
	{
		private bool _removePrefixes;
		private readonly HttpMethod[] _allPrefixes;

		public MethodNameRouteConvention()
		{
			_removePrefixes = true;
			_allPrefixes = new[]
			{
				HttpMethod.Delete,
				HttpMethod.Get,
				HttpMethod.Head,
				HttpMethod.Options,
				HttpMethod.Post,
				HttpMethod.Put,
				HttpMethod.Trace
			};
		}

		public void Execute(TypedRouteBuilder template)
		{
			var name = template.Method.Name;

			if (_removePrefixes)
			{
				var prefix = _allPrefixes.FirstOrDefault(p => name.StartsWith(p.Method, StringComparison.OrdinalIgnoreCase));

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
			
			template.Parts.Add(name);
		}

		public MethodNameRouteConvention DontStripVerbPrefixes()
		{
			_removePrefixes = false;
			return this;
		}
	}
}
