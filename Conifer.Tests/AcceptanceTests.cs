using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using AppStart;
using Conifer;
using Microsoft.Owin.Testing;
using Models;
using Newtonsoft.Json;
using Owin;
using Shouldly;
using StructureMap;
using StructureMap.Graph;
using Xunit;

namespace Conifer.Tests
{
	public class AcceptanceTests : IDisposable
	{
		private readonly TestServer _server;

		public AcceptanceTests()
		{
			_server = TestServer.Create(appBuilder =>
			{
				var config = new HttpConfiguration();
				ApiStart.Register(config);

				appBuilder.UseWebApi(config);
			});
		}

		private RestModel MakeRequest(string path)
		{
			var json = _server.HttpClient.GetStringAsync(path).Result;

			return JsonConvert.DeserializeObject<RestModel>(json);
		}

		[Fact]
		public void Root_controller_should_link_to_self()
		{
			MakeRequest("").Links["self"].ShouldBe("");
		}

		[Fact]
		public void Root_controller_should_link_to_all_books()
		{
			MakeRequest("").Links["allBooks"].ShouldBe("Books/AllBooks");
		}

		[Fact]
		public void All_books_should_link_to_self()
		{
			MakeRequest("Books/AllBooks").Links["self"].ShouldBe("Books/AllBooks");
		}

		[Fact]
		public void All_books_should_link_to_first_book()
		{
			MakeRequest("Books/AllBooks").Links["first"].ShouldBe("Books/ByID/1324");
		}

		[Fact]
		public void Books_by_id_should_link_to_self()
		{
			MakeRequest("Books/ByID/1234").Links["self"].ShouldBe("Books/ByID/1234");
		}

		[Fact]
		public void Books_by_id_should_link_to_all()
		{
			MakeRequest("Books/ByID/1234").Links["all"].ShouldBe("Books/AllBooks");
		}

		[Fact]
		public void Books_by_id_should_link_to_next()
		{
			MakeRequest("Books/ByID/1234").Links["next"].ShouldBe("Books/ByID/1235");
		}

		public void Dispose()
		{
			_server.Dispose();
		}
	}
}
