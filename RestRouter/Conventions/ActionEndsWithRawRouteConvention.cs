namespace RestRouter.Conventions
{
	public class ActionEndsWithRawRouteConvention : IRouteConvention
	{
		public void Execute(RouteTemplateBuilder template)
		{
			if (template.Method.Name.EndsWith("Raw"))
			{
				template.Parts.Add("raw");
			}
		}
	}
}