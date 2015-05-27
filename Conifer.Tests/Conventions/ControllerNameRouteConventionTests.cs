using System.Web.Http;
using Conifer.Conventions;
using Shouldly;
using Xunit;

namespace Conifer.Tests.Conventions
{
	public class ControllerNameRouteConventionTests : ConventionTests
	{
		public ControllerNameRouteConventionTests()
		{
			Convention = () => new ControllerNameRouteConvention();
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
