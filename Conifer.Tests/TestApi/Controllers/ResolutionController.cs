using System.Web.Http;
using Models;

namespace Controllers
{
	public class ResolutionController : ApiController
	{
		public string Get()
		{
			return "Get Response String";
		}

		public string Get(string first)
		{
			return "Get First Response String";
		}

		public string Get(string first, string second)
		{
			return "Get First Second Response String";
		}

		public string Put([FromBody]ResolutionModel model)
		{
			return "Put Response: " + (model ?? new ResolutionModel()).Name;
		}
	}
}