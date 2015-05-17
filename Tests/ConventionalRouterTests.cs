using System.Web.Http;
using Conifer;
using Shouldly;
using Tests.Controllers;
using Xunit;

namespace Tests
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

			router.ShouldSatisfyAllConditions(
				() => router.LinkTo<HomeController>(c => c.Get()).ShouldBe(""),
				() => router.LinkTo<PersonController>(c => c.Get()).ShouldBe("Person"),
				() => router.LinkTo<PersonController>(c => c.GetByID(123)).ShouldBe("Person/ByID/123")
			);
		}

		[Fact]
		public void When_a_controller_has_overloaded_methods()
		{
			var config = new HttpConfiguration();
			var router = Router.Create(config, r =>
			{
				r.Add<MultiMethodController>();
			});

			router.ShouldSatisfyAllConditions(
				() => router.LinkTo<MultiMethodController>(c => c.Get()).ShouldBe("MultiMethod"),
				() => router.LinkTo<MultiMethodController>(c => c.Get(123)).ShouldBe("MultiMethod/123")
			);


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
	}
}
