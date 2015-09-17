using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Conifer.Conventions
{
	public class ParameterNameRouteConvention : IRouteConvention
	{
		public void Execute(TypedRouteBuilder template)
		{
			var parts = template
				.Method
				.GetParameters()
				.Where(p => p.GetCustomAttribute(typeof(FromBodyAttribute)) == null)
				.Select(p => "{" + p.Name + "}")
				.Select(p => new ParameterRoutePart { Value = p });

			template.Parts.AddRange(parts);
		}
	}
}
