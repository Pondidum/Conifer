namespace RestRouter.Conventions
{
	public class MethodNameRouteConvention : IRouteConvention
	{
		public void Execute(TypedRouteBuilder template)
		{
			template.Parts.Add(template.Method.Name);
		}
	}
}
