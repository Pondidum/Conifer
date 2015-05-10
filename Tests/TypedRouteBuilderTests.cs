using System;
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
		private readonly TypedRoute _route;
		private readonly Type _controller;
		private readonly MethodInfo _method;

		public TypedRouteBuilderTests()
		{
			_controller = typeof(IHttpController);
			_method = _controller.GetMethods().First();

			var builder = new TypedRouteBuilder(_controller, _method);
			builder.Parts.Add("First");
			builder.Parts.Add("Second");

			_route = builder.Build(Enumerable.Empty<IRouteConvention>().ToList());
		}

		[Fact]
		public void When_building_a_route_with_parts()
		{
			_route.Template.ShouldBe("First/Second");
		}

		[Fact]
		public void The_controller_is_named()
		{
			_route.ControllerType.ShouldBe(_controller);
		}

		[Fact]
		public void The_action_is_named()
		{
			_route.ActionName.ShouldBe(_method.Name);
		}
	}
}
