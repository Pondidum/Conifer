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
		private readonly List<Action<IConventionalRouter>> _directions;

		public RouterConfigurationExpression()
		{
			_defaultConventions = Default.Conventions.ToList();
			_directions = new List<Action<IConventionalRouter>>();
		}

		/// <summary>Setup the default convetions to create routes</summary>
		public void DefaultConventionsAre(IEnumerable<IRouteConvention> conventions)
		{
			_defaultConventions.Clear();
			_defaultConventions.AddRange(conventions);
		}

		/// <summary>Create routes for all the applicable methods in the controller</summary>
		public void AddAll<TController>()
			where TController : IHttpController
		{
			AddAll<TController>(_defaultConventions);
		}

		/// <summary>Create routes for all the applicable methods in the controller</summary>
		/// <param name="conventions">Override the default conventions with a custom set</param>
		public void AddAll<TController>(IEnumerable<IRouteConvention> conventions)
			where TController : IHttpController
		{
			if (conventions == null) conventions = Enumerable.Empty<IRouteConvention>();

			_directions.Add(router => router.AddRoutes<TController>(conventions.ToList()));
		}

		/// <summary>Creates a route for the specified method in the controller </summary>
		public void Add<TController>(Expression<Action<TController>> expression)
			where TController : IHttpController
		{
			Add(expression, _defaultConventions);
		}

		/// <summary>Creates a route for the specified method in the controller</summary>
		/// <param name="expression">The action to create a route for</param>
		/// <param name="conventions">Override the default conventions with a custom set</param>
		public void Add<TController>(Expression<Action<TController>> expression, IEnumerable<IRouteConvention> conventions)
			where TController : IHttpController
		{
			if (conventions == null) conventions = Enumerable.Empty<IRouteConvention>();

			_directions.Add(router => router.AddRoute(expression, conventions.ToList()));
		}

		/// <summary>Creates routes for all the applicable methods in all of the provided controllers</summary>
		public void AddAllFrom(IEnumerable<Type> controllers)
		{
			foreach (var controller in controllers)
			{
				_directions.Add(router => router.AddRoutes(controller, _defaultConventions));
			}
		}

		public void ApplyTo(IConventionalRouter router)
		{
			_directions.ForEach(direction => direction(router));
		}
	}
}
