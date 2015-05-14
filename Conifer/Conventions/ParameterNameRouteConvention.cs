using System.Linq;

namespace Conifer.Conventions
{
	public class ParameterNameRouteConvention : IRouteConvention
	{
		public void Execute(TypedRouteBuilder template)
		{
			template.Parts.AddRange(template.Method.GetParameters().Select(p => "{" + p.Name + "}"));
		}
	}
}