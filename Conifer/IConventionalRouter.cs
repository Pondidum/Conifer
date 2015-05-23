using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Http.Controllers;

namespace Conifer
{
	public interface IConventionalRouter
	{
		IEnumerable<TypedRoute> Routes { get; }

		void AddRoutes<TController>(List<IRouteConvention> conventions) where TController : IHttpController;

		void AddRoute<TController>(Expression<Action<TController>> expression, List<IRouteConvention> conventions)
			where TController : IHttpController;
	}
}
