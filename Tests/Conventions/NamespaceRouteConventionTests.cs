using System.Linq;
using RestRouter;
using RestRouter.Conventions;
using Shouldly;
using Xunit;

namespace Tests.Conventions
{
	public class NamespaceRouteConventionTests
	{
		private readonly TypedRouteBuilder _builder;

		public NamespaceRouteConventionTests()
		{
			var method = GetType().GetMethod("ToString");
			_builder = new TypedRouteBuilder(typeof(Controller), method);
		}

		[Fact]
		public void The_namespace_should_be_split()
		{
			var convention = new NamespaceRouteConvention();
			convention.Execute(_builder);

			_builder.Parts.ShouldBe(new[] { "Conventions" }.ToList());
		}

		[Fact]
		public void FactMethodName()
		{
			var convention = new NamespaceRouteConvention(ignoreRootNamespace: false);
			convention.Execute(_builder);

			_builder.Parts.ShouldBe(new[] { "Tests", "Conventions" }.ToList());
		}
	}

	internal class Controller { }

}
