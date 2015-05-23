using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http.Controllers;

namespace Conifer
{
	public class RouterConfigurationExpression
	{
		private readonly List<IRouteConvention> _defaultConventions;
		private readonly IConventionalRouter _router;

		public RouterConfigurationExpression(IConventionalRouter router)
		{
			_defaultConventions = Default.Conventions.ToList();
			_router = router;
		}

		/// <summary>
		/// Setup the default convetions to create routes
		/// </summary>
		public void DefaultConventionsAre(IEnumerable<IRouteConvention> conventions)
		{
			_defaultConventions.Clear();
			_defaultConventions.AddRange(conventions);
		}

		/// <summary>
		/// Create routes for all the applicable methods in the controller
		/// </summary>
		public void AddAll<TController>()
			where TController : IHttpController
		{
			AddAll<TController>(_defaultConventions);
		}

		/// <summary>
		/// Create routes for all the applicable methods in the controller
		/// </summary>
		/// <param name="conventions">Override the default conventions with a custom set</param>
		public void AddAll<TController>(IEnumerable<IRouteConvention> conventions)
			where TController : IHttpController
		{
			if (conventions == null) conventions = Enumerable.Empty<IRouteConvention>();

			_router.AddRoutes<TController>(conventions.ToList());
		}

		public void Add<TController>(Expression<Action<TController>> expression)
			where TController : IHttpController
		{
			Add(expression, _defaultConventions);
		}

		public void Add<TController>(Expression<Action<TController>> expression, IEnumerable<IRouteConvention> conventions)
			where TController : IHttpController
		{
			if (conventions == null) conventions = Enumerable.Empty<IRouteConvention>();

			_router.AddRoute(expression, conventions.ToList());
		}
	}
}
