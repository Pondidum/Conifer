using System.Collections.Generic;

namespace Conifer
{
	public static class Extensions
	{
		public static void AddRange<T>(this ICollection<T> self, IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				self.Add(item);
			}
		}
	}
}
