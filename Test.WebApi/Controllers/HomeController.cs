using System.Web.Http;
using RestRouter;

namespace Test.WebApi.Controllers
{
	public class HomeController : ApiController
	{
		private readonly RouteBuilder _router;

		public HomeController(RouteBuilder router)
		{
			_router = router;
		}

		public string Get()
		{
			return "HomeController.Get: " +  _router.RouteFor<PersonController>(p => p.View(123));
		}
	}
}
