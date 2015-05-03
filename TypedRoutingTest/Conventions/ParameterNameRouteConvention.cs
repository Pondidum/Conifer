using System.Linq;

namespace TypedRoutingTest.Conventions
{
	public class ParameterNameRouteConvention : IRouteConvetion
	{
		public void Execute(RouteTemplateBuilder template)
		{
			var parameterNames = template.Method.GetParameters().Select(p => "{" + p.Name + "}");
			template.Parts.Add(string.Join("/", parameterNames));
		}
	}
}