using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Conifer.Conventions;
using Conifer.Tests.Controllers;
using Shouldly;
using Xunit;

namespace Conifer.Tests
{
	public class ConventionalRouterTests
	{
		[Fact]
		public void When_using_the_default_conventions()
		{
			var config = new HttpConfiguration();
			var router = Router.Create(config, r =>
			{
				r.AddAll<HomeController>(null);	//no conventions applied to this route
				r.AddAll<PersonController>();
			});

			router.LinkTo<HomeController>(c => c.Get()).ShouldBe("");
			router.LinkTo<PersonController>(c => c.Get()).ShouldBe("Tests/Person");
			router.LinkTo<PersonController>(c => c.GetByID(123)).ShouldBe("Tests/Person/ByID/123");
		}

		[Fact]
		public void When_a_controller_has_overloaded_methods()
		{
			var config = new HttpConfiguration();
			var router = Router.Create(config, r =>
			{
				r.AddAll<MultiMethodController>();
			});

			router.LinkTo<MultiMethodController>(c => c.Get()).ShouldBe("Tests/MultiMethod");
			router.LinkTo<MultiMethodController>(c => c.Get(123)).ShouldBe("Tests/MultiMethod/123");
		}

		[Fact]
		public void When_adding_a_single_method()
		{
			var config = new HttpConfiguration();
			var router = Router.Create(config, r =>
			{
				r.Add<MultiMethodController>(c => c.Get(123));
			});

			router.LinkTo<MultiMethodController>(c => c.Get(456)).ShouldBe("Tests/MultiMethod/456");
		}

		[Fact]
		public void When_adding_a_single_invalid_method()
		{
			var config = new HttpConfiguration();
			var router = Router.Create(config, r =>
			{
				Should.Throw<ArgumentException>(() => r.Add<InvalidController>(c => c.Get(), Default.Conventions.ToList()));
			});
		}

		[Fact]
		public void When_using_custom_conventions()
		{
			var conventions = new IRouteConvention[]
			{
				new NamespaceRouteConvention().IgnoreThePrefix("Conifer.Tests"),
				new ControllerNameRouteConvention(),
				new MethodNameRouteConvention(),
				new ParameterNameRouteConvention(),
				new RawRouteConvention(),
			};

			var router = new ConventionalRouter();
			router.AddRoutes<RawOverloadController>(conventions.ToList());

			router.Routes.Select(r => r.Template).ShouldBe(new[]
			{
				"RawOverload/ByID/{refnum}",
				"RawOverload/ByID/{refnum}/raw"
			});
		}

		[Fact]
		public void All_routes_get_populated()
		{
			var config = new HttpConfiguration();
			var router = Router.Create(config, r =>
			{
				r.AddAll<PersonController>();
			});

			router.AllRoutes().Select(r => r.Template).ShouldBe(new[]
			{
				"Tests/Person",
				"Tests/Person/ByID/{id}"
			}, ignoreOrder: true);
		}
	}

	namespace Controllers
	{
		public class HomeController : ApiController
		{
			public string Get()
			{
				return "Home.Get";
			}
		}

		public class PersonController : ApiController
		{
			public string Get()
			{
				return "Person.Get";
			}

			public string GetByID(int id)
			{
				return "Person.Get(" + id + ")";
			}
		}

		public class MultiMethodController : ApiController
		{
			public string Get()
			{
				return "Multi.Get";
			}

			public string Get(int id)
			{
				return "Multi.Get(" + id + ")";
			}
		}

		public class InvalidController : ApiController
		{
			public void Get()
			{
			}
		}

		public class RawOverloadController : ApiController
		{
			public string GetByID(int refnum)
			{
				return string.Empty;
			}

			public byte[] GetByIDRaw(int refnum)
			{
				return Enumerable.Empty<byte>().ToArray();
			}
		}
	}
}
