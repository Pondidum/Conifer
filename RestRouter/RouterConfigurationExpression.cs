using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;

namespace RestRouter
{
	public class RouterConfigurationExpression
	{
		private readonly List<IRouteConvention> _defaultConventions;
		private readonly ConventionalRouter _router;

		public RouterConfigurationExpression(TypedDirectRouteProvider routeProvider)
		{
			_defaultConventions = new List<IRouteConvention>();
			_router = new ConventionalRouter(routeProvider);
		}

		public void DefaultConventionsAre(IEnumerable<IRouteConvention> conventions)
		{
			_defaultConventions.Clear();
			_defaultConventions.AddRange(conventions);
		}

		public void Add<TController>() where TController : IHttpController
		{
			Add<TController>(_defaultConventions);
		}

		public void Add<TController>(IEnumerable<IRouteConvention> conventions) where TController : IHttpController
		{
			if (conventions == null) conventions = Enumerable.Empty<IRouteConvention>();

			_router.AddRoutes<TController>(conventions.ToList());
		} 
	}
}
