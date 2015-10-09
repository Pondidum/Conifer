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
			var router = new Router(config, r =>
			{
				r.AddAll<HomeController>(null);	//no conventions applied to this route
				r.AddAll<PersonController>();
			});

			router.LinkTo<HomeController>(c => c.Get()).ShouldBe("");
			router.LinkTo<PersonController>(c => c.Get()).ShouldBe("Tests/Person");
			router.LinkTo<PersonController>(c => c.GetByID(123)).ShouldBe("Tests/Person/ByID/123");

			router.TemplateFor<HomeController>(c => c.Get()).ShouldBe("");
			router.TemplateFor<PersonController>(c => c.Get()).ShouldBe("Tests/Person");
			router.TemplateFor<PersonController>(c => c.GetByID(123)).ShouldBe("Tests/Person/ByID/{id}");
		}

		[Fact]
		public void When_a_controller_has_overloaded_methods()
		{
			var config = new HttpConfiguration();
			var router = new Router(config, r =>
			{
				r.AddAll<MultiMethodController>();
			});

			router.LinkTo<MultiMethodController>(c => c.Get()).ShouldBe("Tests/MultiMethod");
			router.LinkTo<MultiMethodController>(c => c.Get(123)).ShouldBe("Tests/MultiMethod/123");

			router.TemplateFor<MultiMethodController>(c => c.Get()).ShouldBe("Tests/MultiMethod");
			router.TemplateFor<MultiMethodController>(c => c.Get(123)).ShouldBe("Tests/MultiMethod/{id}");
		}

		[Fact]
		public void When_adding_a_single_method()
		{
			var config = new HttpConfiguration();
			var router = new Router(config, r =>
			{
				r.Add<MultiMethodController>(c => c.Get(123));
			});

			router.LinkTo<MultiMethodController>(c => c.Get(456)).ShouldBe("Tests/MultiMethod/456");
			router.TemplateFor<MultiMethodController>(c => c.Get(456)).ShouldBe("Tests/MultiMethod/{id}");
		}

		[Fact]
		public void When_adding_a_single_invalid_method()
		{
			Should.Throw<ArgumentException>(() =>
			{
				var config = new HttpConfiguration();
				var router = new Router(config, r =>
				{
					r.Add<InvalidController>(c => c.Get(), Default.Conventions.ToList());
				});

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
			var router = new ConventionalRouter();
			router.AddRoutes<PersonController>(Default.Conventions.ToList());

			router.Routes.Select(r => r.Template).ShouldBe(new[]
			{
				"Tests/Person",
				"Tests/Person/ByID/{id}"
			}, ignoreOrder: true);
		}

		[Fact]
		public void When_adding_a_route_for_a_non_controller()
		{
			var router = new ConventionalRouter();

			Should.Throw<ArgumentException>(() => router.AddRoutes(typeof(object), new List<IRouteConvention>()));
		}

		[Fact]
		public void When_adding_all_routes_for_multiple_controllers()
		{
			var config = new HttpConfiguration();
			var router = new Router(config, r =>
			{
				r.AddAllFrom(new[] { typeof(HomeController), typeof(PersonController) });
			});

			router.AllRoutes().Select(r=> r.Template).ShouldBe(new[]
			{
				"Tests/Home",
				"Tests/Person",
				"Tests/Person/ByID/{id}"
			});
		}

		[Fact]
		public void When_routing_a_http_and_raw_suffix_only_method()
		{
			var router = new ConventionalRouter();
			router.AddRoutes<DocumentController>(Default.Conventions.ToList());

			router.Routes.Single().Template.ShouldBe("Tests/Document/{id}/raw");
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

		public class DocumentController : ApiController
		{
			public byte[] GetRaw(int id)
			{
				return Enumerable.Empty<byte>().ToArray();
			}
		}
	}
}
