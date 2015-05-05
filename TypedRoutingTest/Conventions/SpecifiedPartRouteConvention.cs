using RestRouter;

namespace TypedRoutingTest.Conventions
{
	public class SpecifiedPartRouteConvention : IRouteConvetion
	{
		private readonly string _part;

		public SpecifiedPartRouteConvention(string part)
		{
			_part = part.TrimEnd('/');
		}

		public void Execute(RouteTemplateBuilder template)
		{
			template.Parts.Add(_part);
		}
	}
}