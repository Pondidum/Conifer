using System.Web.Http;

namespace WebApiDemo.Controllers
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
