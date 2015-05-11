using System.Web.Http;
using RestRouter;

namespace Test.WebApi.Controllers
{
	public class PersonController : ApiController
	{
		public string View(int id)
		{
			return "Person.View(" + id + ")";
		}
	}
}
