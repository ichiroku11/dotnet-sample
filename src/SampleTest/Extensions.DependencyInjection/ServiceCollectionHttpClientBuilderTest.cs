using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace SampleTest.Extensions.DependencyInjection;

public class ServiceCollectionHttpClientBuilderTest {
	[Fact]
	public void AddHttpClient_HttpClientBuilderを取得できる() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		var builder = services.AddHttpClient(name: "abc");

		// Assert
		Assert.Equal("abc", builder.Name);
		// 同じインスタンスではない？
		//Assert.Same(services, builder.Services);
	}

	[Fact]
	public void ConfigureHttpClientDefaults_HttpClientFactoryOptionsを取得できる() {
		// Arrange
		var services = new ServiceCollection();
		services.ConfigureHttpClientDefaults(builder => {
		});

		var provider = services.BuildServiceProvider();

		// Act
		var options = provider.GetRequiredService<IOptions<HttpClientFactoryOptions>>().Value;

		// Assert
		Assert.Empty(options.HttpClientActions);
		Assert.Empty(options.HttpMessageHandlerBuilderActions);
	}
}
