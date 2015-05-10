using System.Collections.Generic;
using RestRouter.Conventions;

namespace RestRouter
{
	public class Default
	{
		public static IEnumerable<IRouteConvention> Conventions { get; private set; }

		static Default()
		{
			Conventions = new IRouteConvention[]
			{
				new NamespaceRouteConvention(), 
				new ControllerNameRouteConvention(),
				new ParameterNameRouteConvention(),
				new ActionEndsWithRawRouteConvention()
			};
		}
	}
}
