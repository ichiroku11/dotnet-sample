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
}
