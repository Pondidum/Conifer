# Conifer

A Convention based router for MVC and WebApi

# Usage

Create a router object inside your configure method:

	public static void Register(HttpConfiguration config)
	{
		var router = Router.Create(config, r =>
		{
			r.Add<HomeController>(null);	//no conventions applied to this route
			r.Add<PersonController>();
		});
	}

The `router` can then be used to generate links to your controller actions:

	// generates a route like `Person/View/123`
	router.LinkTo<PersonController>(p => p.View(123));

A full example can be seen in the [demo webapi project][demo-api].

# Configuration

You can change the route conventions on construction of the router, for example this will facilitate one action per controller:

	var router = Router.Create(config, r =>
	{
		r.DefaultConventionsAre(new IRouteConvention[]
		{
			new ControllerNameRouteConvention(),
			new ParameterNameRouteConvention(),
		});
	}

And generates routes like `Person/1234`


[demo-api]: https://github.com/Pondidum/Conifer/blob/master/WebApiDemo/App_Start/WebApiConfig.cs
