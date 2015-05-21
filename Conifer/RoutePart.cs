namespace Conifer
{
	public class RoutePart
	{
		public string Value { get; set; }
		public PartType Type { get; private set; }

		public RoutePart(PartType type)
		{
			Type = type;
		}
	}
}
