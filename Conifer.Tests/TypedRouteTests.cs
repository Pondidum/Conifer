using System;
using System.Collections.Generic;
using System.Net.Http;
using Controllers;
using Shouldly;
using Xunit;

namespace Conifer.Tests
{
	public class TypedRouteTests
	{
		[Fact]
		public void When_a_non_controller_type_is_passed_in()
		{
			Should.Throw<ArgumentException>(() => new TypedRoute("test", new HashSet<HttpMethod>(),  typeof (Scratchpad), "ToString"));
		}

		[Fact]
		public void When_tostring_is_called()
		{
			var route = new TypedRoute(
				"api/{controller}/{action}",
				new HashSet<HttpMethod> { HttpMethod.Get },
				typeof(BooksController),
				"GetAllBooks");

			route.ToString().ShouldBe("[GET] api/{controller}/{action}");
		}

		[Fact]
		public void When_tostring_is_called_and_there_are_multiple_methods()
		{
			var route = new TypedRoute(
				"api/{controller}/{action}",
				new HashSet<HttpMethod> { HttpMethod.Get, HttpMethod.Post },
				typeof(BooksController),
				"GetAllBooks");

			route.ToString().ShouldBe("[GET POST] api/{controller}/{action}");
		}
	}
}
