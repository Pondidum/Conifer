namespace TypedRoutingTest.Conventions
{
	public class PrefixRouteConvention : IRouteConvetion
	{
		private readonly string _prefix;

		public PrefixRouteConvention(string prefix)
		{
			_prefix = prefix.TrimEnd('/');
		}

		public void Execute(RouteTemplateBuilder template)
		{
			template.Parts.Insert(0, _prefix);
		}
	}
}