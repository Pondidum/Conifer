using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Conifer;
using Conifer.Conventions;
using Shouldly;
using Xunit;

namespace Tests.Conventions
{
	public class MethodNameRouteConventionTests
	{
		private List<string> RunTest(IRouteConvention convention, MethodInfo method)
		{
			var template = new TypedRouteBuilder(typeof(Controller), method);
			convention.Execute(template);

			return template.Parts;
		}

		[Fact]
		public void When_the_method_has_a_known_prefix()
		{
			var method = GetType().GetMethod("GetValue", BindingFlags.NonPublic | BindingFlags.Instance);

			RunTest(new MethodNameRouteConvention(), method).Single().ShouldBe("Value");
		}

		[Fact]
		public void When_the_method_doesnt_start_with_a_known_prefix()
		{
			var method = GetType().GetMethod("PatchValue", BindingFlags.NonPublic | BindingFlags.Instance);

			RunTest(new MethodNameRouteConvention(), method).Single().ShouldBe("PatchValue");
		}

		[Fact]
		public void When_the_method_has_known_prefix_and_prefix_stripping_is_disabled()
		{
			var method = GetType().GetMethod("PatchValue", BindingFlags.NonPublic | BindingFlags.Instance);

			RunTest(new MethodNameRouteConvention().DontStripVerbPrefixes(), method).Single().ShouldBe("PatchValue");
		}

		private void GetValue() { }
		private void PatchValue() { }

		private class Controller { }
	}
}
