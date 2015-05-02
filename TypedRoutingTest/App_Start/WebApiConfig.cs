using System.Web.Http;
using TypedRoutingTest.Controllers;

namespace TypedRoutingTest
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes(new TypedDirectRouteProvider());

			config.TypedRoute("", c => c.Action<HomeController>(h => h.Get()));


			config.TypedRoute(
				"candidate/ref/{refnum:int}",
				c => c.Action<CandidateController>(h => h.GetRef(Arg.Any<int>())));

			config.TypedRoute(
				"candidate/ref/{refnum:int}/{folder}",
				c => c.Action<CandidateController>(h => h.GetRefFolder(Arg.Any<int>(), Arg.Any<string>())));

			config.TypedRoute(
				"candidate/ref/{refnum:int}/{folder}/{file}",
				c => c.Action<CandidateController>(h => h.GetRefFile(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>())));

			config.TypedRoute(
				"candidate/ref/{refnum:int}/{folder}/{file}/raw",
				c => c.Action<CandidateController>(h => h.GetRefFileRaw(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>())));


		}
	}
}
