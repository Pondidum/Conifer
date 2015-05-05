using System.Collections.Generic;
using System.Web.Http;
using RestRouter;
using RestRouter.Conventions;
using Shouldly;
using TypedRoutingTest.Controllers;
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

			var builder = new RouteBuilder(router.Routes);
			var route = builder.RouteFor<CandidateController>(c => c.GetRefFileRaw(GetRef(), "cvs", "file.docx"));

			route.ShouldBe("candidate/ref/456/cvs/file.docx/raw", Case.Insensitive);
		}

		private int GetRef()
		{
			return 456;
		}
	}
}