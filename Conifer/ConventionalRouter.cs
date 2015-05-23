﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;

namespace Conifer
{
	public class ConventionalRouter : IConventionalRouter
	{
		private readonly List<TypedRoute> _routes;

		public ConventionalRouter()
		{
			_routes = new List<TypedRoute>();
		}

		public IEnumerable<TypedRoute> Routes { get { return _routes; } }

		public void AddRoutes<TController>(List<IRouteConvention> conventions) where TController : IHttpController
		{
			var methods = FindMethods<TController>();

			foreach (var template in methods)
			{
				var route = template.Build(conventions);

				_routes.Add(route);
			}
		}

		private static List<TypedRouteBuilder> FindMethods<TController>() where TController : IHttpController
		{
			var type = typeof(TController);

			return type
				.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
				.Where(IsValidAction)
				.Select(m => new TypedRouteBuilder(type, m))
				.ToList();
		}

		private static bool IsValidAction(MethodInfo method)
		{
			return method.IsPublic && method.IsStatic == false && method.ReturnType != typeof (void);
		}
	}
}
