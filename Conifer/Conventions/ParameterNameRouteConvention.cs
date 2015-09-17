using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Conifer.Conventions
{
	public class ParameterNameRouteConvention : IRouteConvention
	{
		private readonly HashSet<string> _names;

		public ParameterNameRouteConvention()
		{
			_names = new HashSet<string>();
		}

		public void Execute(TypedRouteBuilder template)
		{
			var parts = template
				.Method
				.GetParameters()
				.Where(p => _names.Contains(p.Name) == false)
				.Where(p => p.GetCustomAttribute(typeof(FromBodyAttribute)) == null)
				.Select(p => "{" + p.Name + "}")
				.Select(p => new ParameterRoutePart { Value = p });

			template.Parts.AddRange(parts);
		}

		public ParameterNameRouteConvention IgnoreArgumentsCalled(params string[] names)
		{
			_names.Clear();
			_names.AddRange(names);

			return this;
		}

	}
}
