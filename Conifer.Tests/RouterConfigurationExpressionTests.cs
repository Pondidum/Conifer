using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Conifer.Conventions;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Conifer.Tests
{
	public class RouterConfigurationExpressionTests
	{
		private readonly IConventionalRouter _router;
		private readonly RouterConfigurationExpression _expression;

		public RouterConfigurationExpressionTests()
		{
			_router = Substitute.For<IConventionalRouter>();
			_expression = new RouterConfigurationExpression(_router);
		}

		[Fact]
		public void When_no_default_conventions_are_specified()
		{
			_expression.Add<Controller>();

			_router.Received().AddRoutes<Controller>(Arg.Do<List<IRouteConvention>>(x => x.ShouldBe(Default.Conventions)));
		}

		[Fact]
		public void When_no_default_conventions_are_specified_and_individual_are_used()
		{
			_expression.Add<Controller>(new[] { new ControllerNameRouteConvention() });

			_router.Received().AddRoutes<Controller>(Arg.Do<List<IRouteConvention>>(x => x.ShouldBe(new[] { new ControllerNameRouteConvention() })));
		}

		[Fact]
		public void When_no_default_conventions_are_specified_and_null_is_specified()
		{
			_expression.Add<Controller>(null);

			_router.Received().AddRoutes<Controller>(Arg.Do<List<IRouteConvention>>(x => x.ShouldBeEmpty()));
		}

		[Fact]
		public void When_default_conventions_are_specified()
		{
			_expression.DefaultConventionsAre(new[] { new ControllerNameRouteConvention() });
			_expression.Add<Controller>();

			_router.Received().AddRoutes<Controller>(Arg.Do<List<IRouteConvention>>(x => x.ShouldBe(new[] { new ControllerNameRouteConvention() })));
		}

		private class Controller : IHttpController
		{
			public Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
			{
				throw new System.NotImplementedException();
			}
		}
	}
}
