using System.Collections.Generic;

namespace Models
{
	public class RestModel
	{
		public Dictionary<string, string> Links { get; set; }

		public RestModel()
		{
			Links = new Dictionary<string, string>();
		}
	}
}
