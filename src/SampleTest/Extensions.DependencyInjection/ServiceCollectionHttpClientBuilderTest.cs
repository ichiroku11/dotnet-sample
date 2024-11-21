using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace SampleTest.Extensions.DependencyInjection;

public class ServiceCollectionHttpClientBuilderTest {
	[Fact]
	public void ConfigurePrimaryHttpMessageHandler_HttpMessageHandlerBuilderActionsに追加される() {
		// Arrange
		var services = new ServiceCollection();
		services.ConfigureHttpClientDefaults(builder => {
			builder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler());
		});

		var provider = services.BuildServiceProvider();

		// Act
		var options = provider.GetRequiredService<IOptions<HttpClientFactoryOptions>>().Value;

		// Assert
		Assert.Single(options.HttpMessageHandlerBuilderActions);
	}
}
