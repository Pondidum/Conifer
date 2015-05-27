using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Conifer.Conventions;
using Shouldly;
using Xunit;

namespace Conifer.Tests.Conventions
{
	public class MethodNameRouteConventionTests : ConventionTests
	{
		public MethodNameRouteConventionTests()
		{
			Convention = () => new MethodNameRouteConvention();
		}

		[Fact]
		public void When_the_method_has_a_known_prefix()
		{
			ExecuteConventionOn<Controller>(c => c.GetValue());

			Route.ShouldBe("/Value");
			SupportedMethods.ShouldBe(new[] { HttpMethod.Get });
		}

		[Fact]
		public void When_the_method_doesnt_start_with_a_known_prefix()
		{
			ExecuteConventionOn<Controller>(c => c.PatchValue());

			Route.ShouldBe("/PatchValue");
			SupportedMethods.ShouldBeEmpty();
		}

		[Fact]
		public void When_the_method_has_known_prefix_and_prefix_stripping_is_disabled()
		{
			Convention = () => new MethodNameRouteConvention().DontStripVerbPrefixes();
			ExecuteConventionOn<Controller>(c => c.GetValue());

			Route.ShouldBe("/GetValue");
			SupportedMethods.ShouldBeEmpty();
		}

		[Fact]
		public void When_the_name_is_only_a_known_prefix()
		{
			ExecuteConventionOn<Controller>(c => c.Get());

			Route.ShouldBeEmpty();
			SupportedMethods.ShouldBe(new[] { HttpMethod.Get });
		}

		[Fact]
		public void When_using_a_custom_prefix_set()
		{
			Convention = () => new MethodNameRouteConvention().UseCustomPrefixes(new[] { new HttpMethod("Patch") });
			ExecuteConventionOn<Controller>(c => c.PatchValue());

			Route.ShouldBe("/Value");
			SupportedMethods.ShouldBe(new[] { new HttpMethod("PATCH"), });
		}


		private class Controller : ApiController
		{
			public void GetValue() { }
			public void PatchValue() { }
			public void Get() { }
		}
	}
}
