using System.Linq;

namespace Conifer.Conventions
{
	public class ParameterNameRouteConvention : IRouteConvention
	{
		public void Execute(TypedRouteBuilder template)
		{
			var parts = template
				.Method
				.GetParameters()
				.Select(p => "{" + p.Name + "}")
				.Select(p => new ParameterRoutePart { Value = p });

			template.Parts.AddRange(parts);
		}
	}
}