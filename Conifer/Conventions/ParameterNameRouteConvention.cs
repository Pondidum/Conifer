using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Conifer.Conventions
{
	public class ParameterNameRouteConvention : IRouteConvention
	{
		private readonly HashSet<string> _names;
		private bool _matchGreedy;

		public ParameterNameRouteConvention()
		{
			_names = new HashSet<string>();
			_matchGreedy = false;
		}

		public void Execute(TypedRouteBuilder template)
		{
			var parts = template
				.Method
				.GetParameters()
				.Where(p => _names.Contains(p.Name) == false)
				.Where(p => p.GetCustomAttribute(typeof(FromBodyAttribute)) == null)
				.Select(p => p.Name)
				.ToList();

			if (_matchGreedy && parts.Any())
			{
				var last = parts.Last();
				var isGreedy = last.EndsWith("greedy", StringComparison.OrdinalIgnoreCase);

				if (isGreedy)
				{
					parts.Remove(last);
					parts.Add("*" + last);
				}
			}

			template
				.Parts
				.AddRange(parts.Select(p => new ParameterRoutePart { Value = "{" + p + "}" }));
		}

		public ParameterNameRouteConvention IgnoreArgumentsCalled(params string[] names)
		{
			_names.Clear();
			_names.AddRange(names);

			return this;
		}

		public ParameterNameRouteConvention DetectGreedyArguments()
		{
			_matchGreedy = true;
			return this;
		}

	}
}
