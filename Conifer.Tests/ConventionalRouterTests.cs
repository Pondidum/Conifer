using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
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
				r.Add<HomeController>(null);	//no conventions applied to this route
				r.Add<PersonController>();
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
				r.Add<MultiMethodController>();
			});

			router.LinkTo<MultiMethodController>(c => c.Get()).ShouldBe("Tests/MultiMethod");
			router.LinkTo<MultiMethodController>(c => c.Get(123)).ShouldBe("Tests/MultiMethod/123");
		}

		[Fact]
		public void When_adding_a_single_method()
		{
			var router = new ConventionalRouter();
			router.AddRoute<MultiMethodController>(c => c.Get(123), Default.Conventions.ToList());

			router.Routes.Single().Template.ShouldBe("Tests/MultiMethod/{id}");
		}

		[Fact]
		public void When_adding_a_single_invalid_method()
		{
			var router = new ConventionalRouter();
			
			Should.Throw<ArgumentException>(() =>router.AddRoute<InvalidController>(c => c.Get(), Default.Conventions.ToList()));
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
	}
}
