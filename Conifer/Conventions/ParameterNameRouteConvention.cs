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
		private int? _skip;
		private int? _take;

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

			var routeParts = parts.AsEnumerable();

			if (_skip.HasValue)
				routeParts = routeParts.Skip(_skip.Value);

			if (_take.HasValue)
				routeParts = routeParts.Take(_take.Value);

			template
				.Parts
				.AddRange(routeParts.Select(p => new ParameterRoutePart { Value = "{" + p + "}" }));
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

		public ParameterNameRouteConvention Skip(int skip)
		{
			_skip = skip;
			return this;
		}

		public ParameterNameRouteConvention Take(int take)
		{
			_take = take;
			return this;
		}
	}
}
