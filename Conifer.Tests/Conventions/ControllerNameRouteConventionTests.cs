using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Conifer.Conventions;
using Shouldly;
using Xunit;

namespace Conifer.Tests.Conventions
{
	public class ControllerNameRouteConventionTests
	{
		public string Route { get; set; }

		private void ExecuteConventionOn<T>()
		{
			var method = GetType().GetMethod("ToString");

			var template = new TypedRouteBuilder(typeof(T), method);

			var convention = new ControllerNameRouteConvention();
			convention.Execute(template);

			Route = template
				.Parts
				.Select(p => p.Value)
				.Aggregate("", (a, v) => a + "/" + v);
		}

		[Fact]
		public void When_a_controller_is_suffixed_with_controller()
		{
			ExecuteConventionOn<SuffixedController>();
			Route.ShouldBe("/Suffixed");
		}

		[Fact]
		public void When_a_controller_is_suffixed_twice()
		{
			ExecuteConventionOn<TestControllerController>();
			Route.ShouldBe("/TestController");
		}

		[Fact]
		public void When_a_controller_is_not_suffixed()
		{
			ExecuteConventionOn<NoSuffix>();
			Route.ShouldBe("/NoSuffix");
		}

		[Fact]
		public void When_a_controller_is_just_called_controller()
		{
			ExecuteConventionOn<Controller>();
			Route.ShouldBeEmpty();
		}

		internal class Controller : ApiController { }
		internal class SuffixedController : ApiController { }
		internal class TestControllerController : ApiController { }
		internal class NoSuffix : ApiController { }
	}
}
