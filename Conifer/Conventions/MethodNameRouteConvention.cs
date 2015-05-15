using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Conifer.Conventions
{
	public class MethodNameRouteConvention : IRouteConvention
	{
		private bool _removePrefixes;
		private readonly IEnumerable<string> _allPrefixes;

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
			}.Select(p => p.Method);
		}

		public void Execute(TypedRouteBuilder template)
		{
			var name = template.Method.Name;

			if (_removePrefixes)
			{
				var prefix = _allPrefixes.FirstOrDefault(p => name.StartsWith(p, StringComparison.OrdinalIgnoreCase));

				if (string.IsNullOrWhiteSpace(prefix) == false)
				{
					name = name.Substring(prefix.Length);
				}
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
