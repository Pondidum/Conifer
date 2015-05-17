using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Conifer;
using Conifer.Conventions;
using Shouldly;
using Xunit;

namespace Tests.Conventions
{
	public class MethodNameRouteConventionTests
	{
		private TypedRouteBuilder RunTest(IRouteConvention convention, MethodInfo method)
		{
			var template = new TypedRouteBuilder(typeof(Controller), method);
			convention.Execute(template);

			return template;
		}

		[Fact]
		public void When_the_method_has_a_known_prefix()
		{
			var method = GetType().GetMethod("GetValue", BindingFlags.NonPublic | BindingFlags.Instance);
			var builder = RunTest(new MethodNameRouteConvention(), method);

			builder.Parts.Single().ShouldBe("Value");
			builder.SupportedMethods.ShouldBe(new HashSet<HttpMethod>(new[] { HttpMethod.Get }));
		}

		[Fact]
		public void When_the_method_doesnt_start_with_a_known_prefix()
		{
			var method = GetType().GetMethod("PatchValue", BindingFlags.NonPublic | BindingFlags.Instance);
			var builder = RunTest(new MethodNameRouteConvention(), method);

			builder.Parts.Single().ShouldBe("PatchValue");
			builder.SupportedMethods.ShouldBeEmpty();
		}

		[Fact]
		public void When_the_method_has_known_prefix_and_prefix_stripping_is_disabled()
		{
			var method = GetType().GetMethod("PatchValue", BindingFlags.NonPublic | BindingFlags.Instance);
			var builder = RunTest(new MethodNameRouteConvention().DontStripVerbPrefixes(), method);

			builder.Parts.Single().ShouldBe("PatchValue");
			builder.SupportedMethods.ShouldBeEmpty();
		}

		[Fact]
		public void When_the_name_is_only_a_known_prefix()
		{
			var method = GetType().GetMethod("Get", BindingFlags.NonPublic | BindingFlags.Instance);
			var builder = RunTest(new MethodNameRouteConvention(), method);

			builder.Parts.ShouldBeEmpty();
			builder.SupportedMethods.ShouldBe(new HashSet<HttpMethod>(new[] { HttpMethod.Get }));
		}

		[Fact]
		public void When_using_a_custom_prefix_set()
		{
			var method = GetType().GetMethod("PatchValue", BindingFlags.NonPublic | BindingFlags.Instance);
			var builder = RunTest(new MethodNameRouteConvention().UseCustomPrefixes(new[] { new HttpMethod("Patch"), }), method);

			builder.Parts.Single().ShouldBe("Value");
			builder.SupportedMethods.ShouldBe(new HashSet<HttpMethod>(new[] { new HttpMethod("PATCH"), }));
		}

		private void GetValue() { }
		private void PatchValue() { }
		private void Get() { }

		private class Controller { }
	}
}
