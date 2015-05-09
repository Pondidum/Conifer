namespace RestRouter
{
	public interface IRouteConvention
	{
		void Execute(RouteTemplateBuilder template);
	}
}