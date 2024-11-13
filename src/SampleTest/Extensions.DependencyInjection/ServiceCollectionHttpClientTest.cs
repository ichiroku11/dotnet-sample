using Microsoft.Extensions.DependencyInjection;

namespace SampleTest.Extensions.DependencyInjection;

public class ServiceCollectionHttpClientTest {
	[Fact]
	public void AddHttpClient_HttpClientインスタンスを取得できる() {
		// Arrange
		var services = new ServiceCollection();
		services.AddHttpClient();

		var provider = services.BuildServiceProvider();

		// Act
		var client = provider.GetRequiredService<HttpClient>();

		// Assert
		Assert.IsType<HttpClient>(client);
	}

	// todo: HttpClientを複数取得すると同じインスタンス？

	[Fact]
	public void AddHttpClient_IHttpClientFactoryを実装するインスタンスを取得できる() {
		// Arrange
		var services = new ServiceCollection();
		services.AddHttpClient();

		var provider = services.BuildServiceProvider();

		// Act
		var factory = provider.GetRequiredService<IHttpClientFactory>();

		// Assert
		Assert.IsAssignableFrom<IHttpClientFactory>(factory);
	}

	[Fact]
	public void AddHttpClient_IHttpMessageHandlerFactoryを実装するインスタンスを取得できる() {
		// Arrange
		var services = new ServiceCollection();
		services.AddHttpClient();

		var provider = services.BuildServiceProvider();

		// Act
		var factory = provider.GetRequiredService<IHttpMessageHandlerFactory>();

		// Assert
		Assert.IsAssignableFrom<IHttpMessageHandlerFactory>(factory);
	}
}
