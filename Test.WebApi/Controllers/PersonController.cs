using System.Web.Http;
using RestRouter;

namespace Test.WebApi.Controllers
{
	public class PersonController : ApiController
	{
		[HttpGet]
		public string View()
		{
			return "Person.Index";
		}

		[HttpGet]
		public string View(int id)
		{
			return "Person.View(" + id + ")";
		}
	}
}
