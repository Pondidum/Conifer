using System.Net.Http;
using System.Web.Http;

namespace TypedRoutingTest.Controllers
{
	public class HomeController : ApiController
	{
		public HttpResponseMessage Get()
		{
			return new HttpResponseMessage
			{
				Content = new StringContent("Home.Get")
			};
		}
	}
}
