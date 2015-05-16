using System;
using System.Collections.Generic;
using System.Net.Http;
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
			Should.Throw<ArgumentException>(() => new TypedRoute("test", new HashSet<HttpMethod>(),  typeof (Scratchpad), "ToString"));
		}
	}
}
