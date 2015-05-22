using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Conifer.Conventions;
using Shouldly;
using Xunit;

namespace Conifer.Tests.Conventions
{
	public class RawRouteConventionTests
	{
		private List<string> RunTest(string methodName)
		{
			return RunTest(methodName, Enumerable.Empty<RoutePart>());
		}

		private List<string> RunTest(string methodName, IEnumerable<RoutePart> initialParts)
		{
			var method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

			var template = new TypedRouteBuilder(typeof(Controller), method);
			template.Parts.AddRange(initialParts);

			var convention = new RawRouteConvention();
			convention.Execute(template);

			return template
				.Parts
				.Select(p => p.Value)
				.ToList();
		}

		[Fact]
		public void When_the_action_ends_in_raw()
		{
			RunTest("GetDocumentRaw").Single().ShouldBe("raw");
		}

		[Fact]
		public void When_the_action_doesnt_end_in_raw()
		{
			RunTest("GetDocument").ShouldBeEmpty();
		}

		[Fact]
		public void When_there_is_an_action_ending_in_raw()
		{
			RunTest("GetDocumentRaw", new[] { new ActionRoutePart { Value = "GetDocumentRaw" }  })
				.First().ShouldBe("GetDocument");
		}

		private class Controller { }

		private void GetDocumentRaw() { }
		private void GetDocument() { }
	}
}
