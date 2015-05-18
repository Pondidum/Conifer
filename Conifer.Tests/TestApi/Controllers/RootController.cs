using System.Web.Http;
using Conifer;
using Models;

namespace Controllers
{
	public class RootController : ApiController
	{
		private readonly RouteBuilder _router;

		public RootController(RouteBuilder router)
		{
			_router = router;
		}

		public RestModel Get()
		{
			return new RestModel
			{
				Links =
				{
					{ "self", _router.LinkTo<RootController>(c => c.Get()) },
					{ "allBooks", _router.LinkTo<BooksController>(c => c.GetAllBooks()) }
				}
			};
		}
	}
}
