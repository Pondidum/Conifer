using System;
using System.Net.Http;
using Xunit;

namespace Conifer.Tests
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