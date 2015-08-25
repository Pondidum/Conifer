using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Conifer.Conventions;
using Shouldly;
using Xunit;

namespace Conifer.Tests.Conventions
{
	public class RawRouteConventionTests : ConventionTests
	{
		public RawRouteConventionTests()
		{
			Convention = () => new RawRouteConvention();
		}

		[Fact]
		public void When_the_action_doesnt_end_in_raw()
		{
			ExecuteConventionOn<Controller>(c => c.GetDocument());
			Route.ShouldBeEmpty();
		}

		[Fact]
		public void When_the_action_ends_in_raw()
		{
			ExecuteConventionOn<Controller>(c => c.GetDocumentRaw());
			Route.ShouldBe("/raw");
		}

		[Fact]
		public void When_there_is_an_action_in_the_route_ending_in_raw()
		{
			PreConfigure = template => template.Parts.Add(new ActionRoutePart {Value = "GetDocumentRaw"});

			ExecuteConventionOn<Controller>(c => c.GetDocumentRaw());
			Route.ShouldBe("/GetDocument/raw");
		}

		[Fact]
		public void When_the_raw_method_has_only_an_http_method_and_raw_suffix()
		{
			// the method was called GetRaw, the name convention has removed "Get" from it
			PreConfigure = template => template.Parts.AddRange(new RoutePart[] {
				new ActionRoutePart { Value = "Raw" },
				new ParameterRoutePart { Value = "123" }
			});

			ExecuteConventionOn<RawController>(c => c.GetRaw(123));
			Route.ShouldBe("/123/raw");
		}

		private class Controller : ApiController
		{
			public void GetDocumentRaw() { }
			public void GetDocument() { }
		}

		private class RawController : ApiController
		{
			public void GetRaw(int id) { }
		}
	}
}
