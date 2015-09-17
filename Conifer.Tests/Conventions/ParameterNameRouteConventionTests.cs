using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Conifer.Conventions;
using Shouldly;
using Xunit;

namespace Conifer.Tests.Conventions
{
	public class ParameterNameRouteConventionTests : ConventionTests
	{
		public ParameterNameRouteConventionTests()
		{
			Convention = () => new ParameterNameRouteConvention();
		}

		[Fact]
		public void When_the_method_has_no_arguments()
		{
			ExecuteConventionOn<Controller>(c => c.NoArguments());
			Route.ShouldBeEmpty();
		}

		[Fact]
		public void When_the_method_has_one_argument()
		{
			ExecuteConventionOn<Controller>(c => c.OneArgument("a"));
			Route.ShouldBe("/{test}");
		}

		[Fact]
		public void When_the_method_has_two_arguments()
		{
			ExecuteConventionOn<Controller>(c => c.TwoArguments("a", 1));
			Route.ShouldBe("/{text}/{value}");
		}

		[Fact]
		public void When_the_method_has_a_param_array_argument()
		{
			ExecuteConventionOn<Controller>(c => c.ParamArgument());
			Route.ShouldBe("/{items}");
		}

		[Fact]
		public void When_the_method_has_a_frombody_attributed_argument()
		{
			ExecuteConventionOn<Controller>(c => c.BodyArgument("testing"));
			Route.ShouldBeEmpty();
		}

		[Fact]
		public void When_ignoring_arguments_with_a_match()
		{
			Convention = ()=> new ParameterNameRouteConvention().IgnoreArgumentsCalled("text");
			ExecuteConventionOn<Controller>(c => c.TwoArguments("val", 1));
			Route.ShouldBe("/{value}");
		}

		private class Controller : ApiController
		{
			public void NoArguments() { }
			public void OneArgument(string test) { }
			public void TwoArguments(string text, int value) { }
			public void ParamArgument(params string[] items) { }
			public void BodyArgument([FromBody]string value) { }
		}

	}
}
