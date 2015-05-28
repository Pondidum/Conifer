using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Conifer.Conventions;
using Shouldly;
using Xunit;

namespace Conifer.Tests.Conventions
{
	public class NamespaceRouteConventionTests : ConventionTests
	{
		public NamespaceRouteConventionTests()
		{
			Convention = () => new NamespaceRouteConvention();
		}

		[Fact]
		public void The_namespace_should_be_split()
		{
			ExecuteConventionOn<Controllers.Controller>();
			Route.ShouldBe("/Tests/Conventions");
		}

		[Fact]
		public void When_not_ignoring_the_root_namespace()
		{
			Convention = () => new NamespaceRouteConvention().DontIgnoreRootNamespace();

			ExecuteConventionOn<Controllers.Controller>();
			Route.ShouldBe("/Conifer/Tests/Conventions");
		}

		[Fact]
		public void When_not_ignoring_the_controller_namespace()
		{
			Convention = () => new NamespaceRouteConvention().DontIgnoreControllersNamespace();
			ExecuteConventionOn<Controllers.Controller>();

			Route.ShouldBe("/Tests/Conventions/Controllers");
		}

		[Fact]
		public void When_ignoring_a_namespace_prefix()
		{
			Convention = () => new NamespaceRouteConvention().IgnoreThePrefix("Conifer.Tests.Conventions");
			ExecuteConventionOn<Candidates.CandidateController>();

			Route.ShouldBe("/Candidates");
		}
	}

	namespace Controllers
	{
		internal class Controller : ApiController { }
	}

	namespace Candidates
	{
		internal class CandidateController : ApiController { }
	}
}
