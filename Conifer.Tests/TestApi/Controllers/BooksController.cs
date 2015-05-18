using System.Web.Http;
using Conifer;
using Models;

namespace Controllers
{
	public class BooksController : ApiController
	{
		private readonly RouteBuilder _router;

		public BooksController(RouteBuilder router)
		{
			_router = router;
		}

		public RestModel GetAllBooks()
		{
			return new RestModel
			{
				Links =
					{
						{ "self", _router.LinkTo<BooksController>(c => c.GetAllBooks()) },
						{ "first", _router.LinkTo<BooksController>(c => c.GetByID(1324)) }
					}
			};
		}

		public RestModel GetByID(int id)
		{
			return new RestModel
			{
				Links =
					{
						{ "self", _router.LinkTo<BooksController>(c => c.GetByID(id)) },
						{ "all", _router.LinkTo<BooksController>(c => c.GetAllBooks()) },
						{ "next", _router.LinkTo<BooksController>(c => c.GetByID(id + 1)) }
					}
			};
		}
	}
}
