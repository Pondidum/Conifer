using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Conifer.Conventions;
using Shouldly;
using Xunit;

namespace Conifer.Tests.Conventions
{
	public class SpecifiedPartRouteConventionTests : ConventionTests
	{
		[Fact]
		public void When_the_part_is_null()
		{
			Convention = () => new SpecifiedPartRouteConvention(null);
			ExecuteConventionOn<Controller>();
			Route.ShouldBeEmpty();
		}

		[Fact]
		public void When_the_part_is_blank()
		{
			Convention = () => new SpecifiedPartRouteConvention("");
			ExecuteConventionOn<Controller>();
			Route.ShouldBeEmpty();
		}

		[Fact]
		public void When_the_part_is_whitespace()
		{
			Convention = () => new SpecifiedPartRouteConvention("    ");
			ExecuteConventionOn<Controller>();
			Route.ShouldBeEmpty();
		}

		[Fact]
		public void When_the_part_is_specified()
		{
			Convention = () => new SpecifiedPartRouteConvention("testing");
			ExecuteConventionOn<Controller>();
			Route.ShouldBe("/testing");
		}

		[Fact]
		public void When_the_part_starts_and_ends_with_a_slash()
		{
			Convention = () => new SpecifiedPartRouteConvention("/test/");
			ExecuteConventionOn<Controller>();
			Route.ShouldBe("/test");
		}

		[Fact]
		public void When_the_part_contains_a_slash()
		{
			Convention = () => new SpecifiedPartRouteConvention("first/second");
			ExecuteConventionOn<Controller>();
			Route.ShouldBe("/first/second");
		}

		private class Controller : ApiController { }
	}
}
