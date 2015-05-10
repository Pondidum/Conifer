using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestRouter;
using RestRouter.Conventions;
using Shouldly;
using Xunit;

namespace Tests.Conventions
{
	public class ParameterNameRouteConventionTests
	{
		private List<string> RunTest(string methodName)
		{
			var method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

			var template = new TypedRouteBuilder(typeof(Controller), method);

			var convention = new ParameterNameRouteConvention();
			convention.Execute(template);

			return template.Parts;
		}

		[Fact]
		public void When_the_method_has_no_arguments()
		{
			RunTest("NoArguments").ShouldBeEmpty();
		}

		[Fact]
		public void When_the_method_has_one_argument()
		{
			RunTest("OneArgument").Single().ShouldBe("{test}");
		}

		[Fact]
		public void When_the_method_has_two_arguments()
		{
			RunTest("TwoArguments").ShouldBe(new[] { "{text}", "{value}" });

		}

		[Fact]
		public void When_the_method_has_a_param_array_argument()
		{
			RunTest("ParamArgument").Single().ShouldBe("{items}");
		}

		private class Controller { }

		private void NoArguments() { }
		private void OneArgument(string test) { }
		private void TwoArguments(string text, int value) { }
		private void ParamArgument(params string[] items) { }
	}
}
