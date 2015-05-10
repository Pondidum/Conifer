namespace RestRouter
{
	public interface IRouteConvention
	{
		void Execute(TypedRouteBuilder template);
	}
}