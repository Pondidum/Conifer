using System.Web.Http;
using Conifer;

namespace WebApiDemo.Controllers
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
			return "HomeController.Get: " +  _router.LinkTo<PersonController>(p => p.View(123));
		}
	}
}
