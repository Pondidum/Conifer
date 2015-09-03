using System.Web.Http;
using Conifer;
using Models;

namespace Controllers
{
	public class RootController : ApiController
	{
		private readonly Router _router;

		public RootController(Router router)
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
