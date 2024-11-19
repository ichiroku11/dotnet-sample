using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

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

	[Fact]
	public void AddHttpClient_GetRequiredServiceを呼び出すごとに異なるHttpClientインスタンスを取得できる() {
		// Arrange
		var services = new ServiceCollection();
		services.AddHttpClient();

		var provider = services.BuildServiceProvider();

		// Act
		var client1 = provider.GetRequiredService<HttpClient>();
		var client2 = provider.GetRequiredService<HttpClient>();

		// Assert
		Assert.NotSame(client1, client2);
	}

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

	[Fact]
	public void AddHttpClient_HttpClientFactoryOptionsのインスタンスを取得できる() {
		// Arrange
		var services = new ServiceCollection();
		services.AddHttpClient();

		var provider = services.BuildServiceProvider();
		// Act

		var options = provider.GetRequiredService<IOptions<HttpClientFactoryOptions>>().Value;

		// Assert
		Assert.NotNull(options);
		Assert.Empty(options.HttpClientActions);
		Assert.Empty(options.HttpMessageHandlerBuilderActions);
	}
}
