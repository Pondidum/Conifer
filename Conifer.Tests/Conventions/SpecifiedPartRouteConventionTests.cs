using System.Collections.Generic;
using System.Linq;
using Conifer.Conventions;
using Shouldly;
using Xunit;

namespace Conifer.Tests.Conventions
{
	public class SpecifiedPartRouteConventionTests
	{
		private List<string> RunTest(string part)
		{
			var method = GetType().GetMethod("ToString");

			var template = new TypedRouteBuilder(typeof(Controller), method);

			var convention = new SpecifiedPartRouteConvention(part);
			convention.Execute(template);

			return template.Parts;
		}

		[Fact]
		public void When_the_part_is_null()
		{
			RunTest(null).ShouldBeEmpty();
		}

		[Fact]
		public void When_the_part_is_blank()
		{
			RunTest("").ShouldBeEmpty();
		}

		[Fact]
		public void When_the_part_is_whitespace()
		{
			RunTest("    ").ShouldBeEmpty();
		}

		[Fact]
		public void When_the_part_is_specified()
		{
			RunTest("testing").Single().ShouldBe("testing");
		}

		[Fact]
		public void When_the_part_starts_and_ends_with_a_slash()
		{
			RunTest("/test/").Single().ShouldBe("test");
		}

		[Fact]
		public void When_the_part_contains_a_slash()
		{
			RunTest("first/second").Single().ShouldBe("first/second");
		}

		private class Controller { }
	}
}
