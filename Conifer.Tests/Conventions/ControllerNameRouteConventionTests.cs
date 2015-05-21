﻿using System.Collections.Generic;
using System.Linq;
using Conifer.Conventions;
using Shouldly;
using Xunit;

namespace Conifer.Tests.Conventions
{
	public class ControllerNameRouteConventionTests
	{
		private List<string> RunTest<T>()
		{
			var method = GetType().GetMethod("ToString");

			var template = new TypedRouteBuilder(typeof(T), method);

			var convention = new ControllerNameRouteConvention();
			convention.Execute(template);

			return template
				.Parts
				.Select(p => p.Value)
				.ToList();
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

		[Fact]
		public void When_a_controller_is_just_called_controller()
		{
			RunTest<Controller>().ShouldBeEmpty();
		}

		internal class Controller { }
		internal class SuffixedController { }
		internal class TestControllerController { }
		internal class NoSuffix { }
	}
}
