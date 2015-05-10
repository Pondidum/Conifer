﻿using System;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using RestRouter;
using Shouldly;
using Xunit;

namespace Tests
{
	public class TypedRouteBuilderTests
	{
		private readonly TypedRouteBuilder _builder;
		private readonly Type _controller;
		private readonly MethodInfo _method;

		public TypedRouteBuilderTests()
		{
			_controller = typeof(IHttpController);
			_method = _controller.GetMethods().First();

			_builder = new TypedRouteBuilder(_controller, _method);
			_builder.Parts.Add("First");
			_builder.Parts.Add("Second");
		}

		[Fact]
		public void When_building_a_route_with_parts()
		{
			var route = _builder.Build(Enumerable.Empty<IRouteConvention>().ToList());

			route.ShouldSatisfyAllConditions(
				() => route.Template.ShouldBe("First/Second"),
				() => route.ControllerType.ShouldBe(_controller),
				() => route.ActionName.ShouldBe(_method.Name)
			);
		}

		[Fact]
		public void When_conventions_are_specified()
		{
			var route = _builder.Build(new IRouteConvention[] {new TestConvention()}.ToList());

			route.Template.ShouldBeEmpty();
		}

		private class TestConvention : IRouteConvention
		{
			public void Execute(TypedRouteBuilder template)
			{
				template.Parts.Clear();
			}
		}
	}
}