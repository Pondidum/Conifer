using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestRouter;
using RestRouter.Conventions;
using Shouldly;
using Xunit;

namespace Tests.Conventions
{
	public class ActionEndsWithRawRouteConventionTests
	{
		private List<string> RunTest(string methodName)
		{
			var method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

			var template = new RouteTemplateBuilder(typeof(Controller), method);

			var convention = new ActionEndsWithRawRouteConvention();
			convention.Execute(template);

			return template.Parts;
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

		private class Controller { }

		private void GetDocumentRaw() { }
		private void GetDocument() { }
	}
}
