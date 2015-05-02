using System.Net.Http;
using System.Web.Http;

namespace TypedRoutingTest.Controllers
{
	public class CandidateController : ApiController
	{
		//  candidate/ref/{refnum} => List<FileModel>
		//  candidate/ref/{refnum}/{folder} => List<FileModel>
		//  candidate/ref/{refnum}/{folder}/{file} => FileModel
		//  candidate/ref/{refnum}/{folder}/{file}/raw => binary

		public string GetRef(int refnum)
		{
			return "candidate/ref/{refnum}";
		}

		public string GetRefFolder(int refnum, string folder)
		{
			return "candidate/ref/{refnum}/{folder}";
		}

		public string GetRefFile(int refnum, string folder, string file)
		{
			return "candidate/ref/{refnum}/{folder}/{file}";
		}

		public string GetRefFileRaw(int refnum, string folder, string file)
		{
			return "candidate/ref/{refnum}/{folder}/{file}/raw";
		}
	}
}
