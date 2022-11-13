using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Encodings.Web;

namespace SampleTest.AspNetCore;

// 参考
// https://github.com/dotnet/aspnetcore/blob/c85baf8db0c72ae8e68643029d514b2e737c9fae/src/Mvc/Mvc.Core/test/Routing/ControllerLinkGeneratorExtensionsTest.cs
public class LinkGeneratorTest {

	private static RouteEndpoint CreateEndpoint() {
		return new RouteEndpoint(
			requestDelegate: (httpContext) => Task.CompletedTask,
			routePattern: RoutePatternFactory.Parse(
				pattern: "{controller}/{action}/{id?}",
				defaults: new { controller = "Default", action = "Index" },
				parameterPolicies: null,
				requiredValues: new { controller = "Default", action = "Index" }),
			order: 0,
			metadata: null,
			displayName: null);
	}

	private static IServiceProvider CreateServiceProvider(params Endpoint[] endpoints) {
		var services = new ServiceCollection();
		services.AddOptions();
		services.AddLogging();
		services.AddRouting();
		services.AddSingleton(UrlEncoder.Default);
		services.TryAddEnumerable(ServiceDescriptor.Singleton<EndpointDataSource>(new DefaultEndpointDataSource(endpoints)));

		return services.BuildServiceProvider();
	}

	private static LinkGenerator CreateLinkGenerator() {
		var services = CreateServiceProvider(CreateEndpoint());

		return services.GetRequiredService<LinkGenerator>();
	}

	[Fact]
	public void GetPathByAction_絶対パスを確認する() {
		// Arrange
		var linkGenerator = CreateLinkGenerator();

		// Act
		var actual = linkGenerator.GetPathByAction("Index", "Default");

		// Assert
		Assert.Equal("/", actual);
	}
}
