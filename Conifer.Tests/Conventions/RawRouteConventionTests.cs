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
		public void When_the_action_ends_in_raw()
		{
			ExecuteConventionOn<Controller>(c => c.GetDocumentRaw());
			Route.ShouldBe("/raw");
		}

		[Fact]
		public void When_the_action_doesnt_end_in_raw()
		{
			ExecuteConventionOn<Controller>(c => c.GetDocument());
			Route.ShouldBeEmpty();
		}

		[Fact]
		public void When_there_is_an_action_in_the_route_ending_in_raw()
		{
			PreConfigure = template => template.Parts.Add(new ActionRoutePart {Value = "GetDocumentRaw"});

			ExecuteConventionOn<Controller>(c => c.GetDocumentRaw());
			Route.ShouldBe("/GetDocument/raw");
		}

		private class Controller : ApiController
		{
			public void GetDocumentRaw() { }
			public void GetDocument() { }
		}
	}
}
