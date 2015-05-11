using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using StructureMap;

namespace Test.WebApi
{
	class StructureMapDependencyScope : IDependencyScope
	{
		public IContainer Container { get; private set; }

		public StructureMapDependencyScope(IContainer container)
		{
			Container = container;
		}

		public object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				return null;
			}

			if (serviceType.IsAbstract || serviceType.IsInterface)
			{
				return Container.TryGetInstance(serviceType);
			}

			return Container.GetInstance(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return Container.GetAllInstances(serviceType).Cast<Object>();
		}

		public void Dispose()
		{
			Container.Dispose();
			Container = null;
		}
	}
}
