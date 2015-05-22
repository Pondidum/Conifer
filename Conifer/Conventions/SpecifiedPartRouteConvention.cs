namespace Conifer.Conventions
{
	public class SpecifiedPartRouteConvention : IRouteConvention
	{
		private readonly string _part;

		public SpecifiedPartRouteConvention(string part)
		{
			_part = part;
		}

		public void Execute(TypedRouteBuilder template)
		{
			if (string.IsNullOrWhiteSpace(_part))
			{
				return;
			}

			template.Parts.Add(new ConstantRoutePart { Value = _part.Trim('/') });
		}
	}
}