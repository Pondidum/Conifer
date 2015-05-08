using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestRouter;
using RestRouter.Conventions;
using Shouldly;
using Xunit;

namespace Tests.Conventions
{
	public class ControllerNameRouteConventionTests
	{
		private List<string> RunTest<T>()
		{
			var method = GetType().GetMethod("ToString");

			var template = new RouteTemplateBuilder(typeof(T), method);

			var convention = new ControllerNameRouteConvention();
			convention.Execute(template);

			return template.Parts;
		}

		[Fact]
		public void When_a_controller_is_suffixed_with_controller()
		{
			RunTest<SuffixedController>().Single().ShouldBe("Suffixed");
		}

		[Fact]
		public void When_a_controller_is_suffixed_twice()
		{
			RunTest<TestControllerController>().Single().ShouldBe("TestController");
		}

		[Fact]
		public void When_a_controller_is_not_suffixed()
		{
			RunTest<NoSuffix>().Single().ShouldBe("NoSuffix");
		}

		internal class SuffixedController { }
		internal class TestControllerController { }
		internal class NoSuffix { }
	}
}
