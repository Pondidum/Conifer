using System.Web.Http;

namespace WebApiDemo.Controllers
{
	public class PersonController : ApiController
	{
		public string GetView()
		{
			return "Person.Index";
		}

		public string GetView(int id)
		{
			return "Person.View(" + id + ")";
		}

		public string PostTest()
		{
			return "posted to Person.Test()";
		}

		public string GetTest()
		{
			return "get to Person.Test()";
		}
	}
}
