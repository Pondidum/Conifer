namespace Conifer.Conventions
{
	public class ActionEndsWithRawRouteConvention : IRouteConvention
	{
		public void Execute(TypedRouteBuilder template)
		{
			if (template.Method.Name.EndsWith("Raw"))
			{
				template.Parts.Add("raw");
			}
		}
	}
}