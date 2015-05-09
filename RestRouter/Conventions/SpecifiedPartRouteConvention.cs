namespace RestRouter.Conventions
{
	public class SpecifiedPartRouteConvention : IRouteConvention
	{
		private readonly string _part;

		public SpecifiedPartRouteConvention(string part)
		{
			_part = part;
		}

		public void Execute(RouteTemplateBuilder template)
		{
			if (string.IsNullOrWhiteSpace(_part))
			{
				return;
			}

			template.Parts.Add(_part.Trim('/'));
		}
	}
}