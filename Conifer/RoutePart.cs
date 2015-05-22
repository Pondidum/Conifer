namespace Conifer
{
	public abstract class RoutePart
	{
		public string Value { get; set; }
	}

	public class NamespaceRoutePart : RoutePart { }
	public class ControllerRoutePart : RoutePart { }
	public class ActionRoutePart : RoutePart { }
	public class ParameterRoutePart : RoutePart { }
	public class ConstantRoutePart : RoutePart { }

}
