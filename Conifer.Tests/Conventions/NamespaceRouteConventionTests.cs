using System.Collections.Generic;
using System.Linq;
using Conifer.Conventions;
using Shouldly;
using Xunit;

namespace Conifer.Tests.Conventions
{
	public class NamespaceRouteConventionTests
	{
		[Fact]
		public void The_namespace_should_be_split()
		{
			var result = Test<Controllers.Controller>(new NamespaceRouteConvention());

			result.ShouldBe(new[] {"Tests", "Conventions" }.ToList());
		}

		[Fact]
		public void When_not_ignoring_the_root_namespace()
		{
			var result = Test<Controllers.Controller>(new NamespaceRouteConvention().DontIgnoreRootNamespace());

			result.ShouldBe(new[] { "Conifer", "Tests", "Conventions" }.ToList());
		}

		[Fact]
		public void When_not_ignoring_the_controller_namespace()
		{
			var result = Test<Controllers.Controller>(new NamespaceRouteConvention().DontIgnoreControllersNamespace());

			result.ShouldBe(new[] { "Tests", "Conventions", "Controllers" });
		}

		[Fact]
		public void When_ignoring_a_namespace_prefix()
		{
			var result = Test<Candidates.CandidateController>(new NamespaceRouteConvention().IgnoreThePrefix("Conifer.Tests.Conventions"));

			result.ShouldBe(new[] { "Candidates" });
		}


		private List<string> Test<TController>(NamespaceRouteConvention convention)
		{

			var method = GetType().GetMethod("ToString");
			var builder = new TypedRouteBuilder(typeof(TController), method);

			convention.Execute(builder);

			return builder
				.Parts
				.Select(p => p.Value)
				.ToList();
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
