namespace TypedRoutingTest.Conventions
{
	public class RawOptionRouteConvention : IRouteConvetion
	{
		public void Execute(RouteTemplate template)
		{
			if (template.Method.Name.EndsWith("Raw"))
			{
				template.Parts.Add("raw");
			}
		}
	}
}