using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Conifer;
using Conifer.Conventions;
using Shouldly;
using Xunit;

namespace Tests
{
	public class Scratchpad
	{
		[Fact]
		public void When_testing_something()
		{
			Console.WriteLine(HttpMethod.Get.ToString());
		}
	}
}