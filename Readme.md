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

# Conventions

## ControllerNameRouteConvention
Adds the name of the controller class to the route, removing the suffix "controller" from the class name.

	[Fact]
	public void When_a_controller_is_suffixed_with_controller()
	{
		ExecuteConventionOn<SuffixedController>();
		Route.ShouldBe("/Suffixed");
	}

	[Fact]
	public void When_a_controller_is_not_suffixed()
	{
		ExecuteConventionOn<NoSuffix>();
		Route.ShouldBe("/NoSuffix");
	}

## MethodNameRouteConvention
Adds the name of the method to the route, removing any `HttpMethod` prefixes from the name, and adding them to the supported route actions.

	[Fact]
	public void When_the_method_has_a_known_prefix()
	{
		ExecuteConventionOn<Controller>(c => c.GetValue());

		Route.ShouldBe("/Value");
		SupportedMethods.ShouldBe(new[] { HttpMethod.Get });
	}

	[Fact]
	public void When_the_method_doesnt_start_with_a_known_prefix()
	{
		ExecuteConventionOn<Controller>(c => c.PatchValue());

		Route.ShouldBe("/PatchValue");
		SupportedMethods.ShouldBeEmpty();
	}

### .DontStripVerbPrefixes()
Leaves the method name in tact:

	[Fact]
	public void When_the_method_has_known_prefix_and_prefix_stripping_is_disabled()
	{
		Convention = () => new MethodNameRouteConvention().DontStripVerbPrefixes();
		ExecuteConventionOn<Controller>(c => c.GetValue());

		Route.ShouldBe("/GetValue");
		SupportedMethods.ShouldBeEmpty();
	}

### .UseCustomPrefixes(IEnumerable&lt;HttpMethod&gt; prefixes)
Replaces the list of known prefixes which can be detected with a custom list.

	[Fact]
	public void When_using_a_custom_prefix_set()
	{
		Convention = () => new MethodNameRouteConvention().UseCustomPrefixes(new[] { new HttpMethod("Patch") });
		ExecuteConventionOn<Controller>(c => c.PatchValue());

		Route.ShouldBe("/Value");
		SupportedMethods.ShouldBe(new[] { new HttpMethod("PATCH"), });
	}

## NamespaceRouteConvention
Adds the namespace of the controller to the route, and removes the root assembly's part and `Controllers` part.

	[Fact]
	public void The_namespace_should_be_split()
	{
		ExecuteConventionOn<Controllers.Controller>();
		Route.ShouldBe("/Tests/Conventions");
	}

### .DontIgnoreRootNamespace()
Adds the assebly level namespace part to the route

	[Fact]
	public void When_not_ignoring_the_root_namespace()
	{
		Convention = () => new NamespaceRouteConvention().DontIgnoreRootNamespace();

		ExecuteConventionOn<Controllers.Controller>();
		Route.ShouldBe("/Conifer/Tests/Conventions");
	}

### .DontIgnoreControllersNamespace()
Adds the `Controllers` namespace part in as part of the route

	[Fact]
	public void When_not_ignoring_the_controller_namespace()
	{
		Convention = () => new NamespaceRouteConvention().DontIgnoreControllersNamespace();
		ExecuteConventionOn<Controllers.Controller>();

		Route.ShouldBe("/Tests/Conventions/Controllers");
	}

### .IgnoreThePrefix(string prefix)
Causes a namespace prefix not to be added to the route.  Useful if your assembly name contains periods (e.g. `Conifer.Tests`)

	[Fact]
	public void When_ignoring_a_namespace_prefix()
	{
		Convention = () => new NamespaceRouteConvention().IgnoreThePrefix("Conifer.Tests.Conventions");
		ExecuteConventionOn<Candidates.CandidateController>();

		Route.ShouldBe("/Candidates");
	}

## ParemeterNameRouteConvention
## RawRouteConvention
## SpecifiedPartRouteConvention


[demo-api]: https://github.com/Pondidum/Conifer/blob/master/WebApiDemo/App_Start/WebApiConfig.cs
