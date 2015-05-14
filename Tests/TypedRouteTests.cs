using System;
using Conifer;
using Shouldly;
using Xunit;

namespace Tests
{
	public class TypedRouteTests
	{
		[Fact]
		public void When_a_non_controller_type_is_passed_in()
		{
			Should.Throw<ArgumentException>(() => new TypedRoute("test", typeof (Scratchpad), "ToString"));
		}
	}
}
