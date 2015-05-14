namespace Conifer
{
	public interface IRouteConvention
	{
		void Execute(TypedRouteBuilder template);
	}
}