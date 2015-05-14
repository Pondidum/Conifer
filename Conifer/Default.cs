using System.Collections.Generic;
using Conifer.Conventions;

namespace Conifer
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
				new MethodNameRouteConvention(),
				new ParameterNameRouteConvention(),
				new ActionEndsWithRawRouteConvention()
			};
		}
	}
}
