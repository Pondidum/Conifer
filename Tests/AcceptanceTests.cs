using System.Web.Http;
using RestRouter;
using Shouldly;
using Tests.Controllers;
using Xunit;

namespace Tests
{
	public class AcceptanceTests
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
				() => router.RouteFor<HomeController>(c => c.Get()).ShouldBe(""),
				() => router.RouteFor<PersonController>(c => c.Get()).ShouldBe("Person/Get"),
				() => router.RouteFor<PersonController>(c => c.GetByID(123)).ShouldBe("Person/GetByID/123")
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
	}
}
