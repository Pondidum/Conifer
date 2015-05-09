using System.Linq;

namespace RestRouter.Conventions
{
	public class ParameterNameRouteConvention : IRouteConvention
	{
		public void Execute(RouteTemplateBuilder template)
		{
			template.Parts.AddRange(template.Method.GetParameters().Select(p => "{" + p.Name + "}"));
		}
	}
}