using System.Collections.Generic;
using System.Web.Http;
using Shouldly;
using TypedRoutingTest;
using TypedRoutingTest.Controllers;
using TypedRoutingTest.Conventions;
using Xunit;

namespace Tests
{
	public class Scratchpad
	{
		[Fact]
		public void When_testing_something()
		{
			var conventions = new List<IRouteConvetion>
			{
				new ControllerNameRouteConvention(),
				new SpecifiedPartRouteConvention("ref"),
				new ParameterNameRouteConvention(),
				new RawOptionRouteConvention()
			};

			var config = new HttpConfiguration();
			var router = new ConventionalRouter(config);

			router.AddRoutes<CandidateController>(conventions);

			var builder = new RouteBuilder(router.AllRoutes);
			var route = builder.RouteFor<CandidateController>(c => c.GetRefFileRaw(GetRef(), "cvs", "file.docx"));

			route.ShouldBe("candidate/ref/456/cvs/file.docx/raw", Case.Insensitive);
		}

		private int GetRef()
		{
			return 456;
		}
	}
}