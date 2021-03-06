﻿using System.Collections.Generic;
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
			Convention = () => new ParameterNameRouteConvention().IgnoreArgumentsCalled("text");
			ExecuteConventionOn<Controller>(c => c.TwoArguments("val", 1));
			Route.ShouldBe("/{value}");
		}

		[Fact]
		public void When_detecting_greedy_and_there_are_no_arguments()
		{
			Convention = () => new ParameterNameRouteConvention().DetectGreedyArguments();
			ExecuteConventionOn<GreedyController>(c => c.NoArguments());
			Route.ShouldBeEmpty();
		}

		[Fact]
		public void When_detecting_greedy_and_there_is_one_non_greedy_argument()
		{
			Convention = () => new ParameterNameRouteConvention().DetectGreedyArguments();
			ExecuteConventionOn<GreedyController>(c => c.OneArgument("blah"));
			Route.ShouldBe("/{first}");
		}

		[Fact]
		public void When_detecting_greedy_and_the_first_parameter_is_greedy()
		{
			Convention = () => new ParameterNameRouteConvention().DetectGreedyArguments();
			ExecuteConventionOn<GreedyController>(c => c.TwoArgumentsFirstGreedy("one", "two"));
			Route.ShouldBe("/{firstGreedy}/{lastNot}");
		}

		[Fact]
		public void When_detecting_greedy_and_the_last_parameter_is_greedy()
		{
			Convention = () => new ParameterNameRouteConvention().DetectGreedyArguments();
			ExecuteConventionOn<GreedyController>(c => c.TwoArgumentsLastGreedy("one", "two"));
			Route.ShouldBe("/{firstNot}/{*lastGreedy}");
		}

		[Fact]
		public void When_skipping_a_parameter()
		{
			Convention = () => new ParameterNameRouteConvention().Skip(1);
			ExecuteConventionOn<Controller>(c => c.ThreeArguments("one", "two", "three"));

			Route.ShouldBe("/{second}/{third}");
		}

		[Fact]
		public void When_taking_a_parameter()
		{
			Convention = () => new ParameterNameRouteConvention().Take(1);
			ExecuteConventionOn<Controller>(c => c.ThreeArguments("one", "two", "three"));

			Route.ShouldBe("/{first}");
		}

		private class Controller : ApiController
		{
			public void NoArguments() { }
			public void OneArgument(string test) { }
			public void TwoArguments(string text, int value) { }
			public void ThreeArguments(string first, string second, string third) { }
			public void ParamArgument(params string[] items) { }
			public void BodyArgument([FromBody]string value) { }
		}

		private class GreedyController : ApiController
		{
			public void NoArguments() { }
			public void OneArgument(string first) { }
			public void TwoArgumentsFirstGreedy(string firstGreedy, string lastNot) { }
			public void TwoArgumentsLastGreedy(string firstNot, string lastGreedy) { }
		}

	}
}
