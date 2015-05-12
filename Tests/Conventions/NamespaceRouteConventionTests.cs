using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestRouter;
using RestRouter.Conventions;
using Shouldly;
using Xunit;

namespace Tests.Conventions
{
	public class NamespaceRouteConventionTests
	{
		[Fact]
		public void The_namespace_should_be_split()
		{
			var result = Test<Controllers.Controller>(new NamespaceRouteConvention());

			result.ShouldBe(new[] { "Conventions" }.ToList());
		}

		[Fact]
		public void When_not_ignoring_the_root_namespace()
		{
			var result = Test<Controllers.Controller>(new NamespaceRouteConvention().DontIgnoreRootNamespace());

			result.ShouldBe(new[] { "Tests", "Conventions" }.ToList());
		}

		[Fact]
		public void When_not_ignoring_the_controller_namespace()
		{
			var result = Test<Controllers.Controller>(new NamespaceRouteConvention().DontIgnoreControllersNamespace());

			result.ShouldBe(new[] { "Conventions", "Controllers" });
		}

		[Fact]
		public void When_ignoring_a_namespace_prefix()
		{
			var result = Test<Candidates.CandidateController>(new NamespaceRouteConvention().IgnoreThePrefix("Tests.Conventions"));

			result.ShouldBe(new[] { "Candidates" });
		}


		private List<string> Test<TController>(NamespaceRouteConvention convention)
		{

			var method = GetType().GetMethod("ToString");
			var builder = new TypedRouteBuilder(typeof(TController), method);

			convention.Execute(builder);

			return builder.Parts;
		}
	}

	namespace Controllers
	{
		internal class Controller { }
	}

	namespace Candidates
	{
		internal class CandidateController { }
	}
}
