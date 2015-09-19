using System.Web.Http;

namespace WebApiDemo.Controllers
{
	public class GreedyController : ApiController
	{
		public string GetNoArguments()
		{
			return "NoArguments";
		}

		public string GetOneArgument(string first)
		{
			return "OneArgument";
		}

		public string GetTwoArgumentsFirstGreedy(string firstGreedy, string lastNot)
		{
			return "TwoArgs, first greedy";
		}

		public string GetTwoArgumentsLastGreedy(string firstNot, string lastGreedy)
		{
			return "TwoArgs, last greedy:" + lastGreedy ;
		}
	}
}
