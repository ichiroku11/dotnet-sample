using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace SampleTest.Extensions.DependencyInjection;

public class ServiceCollectionHttpClientBuilderTest {
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ConfigurePrimaryHttpMessageHandler_HttpMessageHandlerBuilderActionsに追加される(bool pattern) {
		// Arrange
		var services = new ServiceCollection();
		services.ConfigureHttpClientDefaults(builder => {
			if (pattern) {
				builder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler());
			} else {
				builder.ConfigurePrimaryHttpMessageHandler<HttpClientHandler>();
			}
		});

		var provider = services.BuildServiceProvider();

		// Act
		var options = provider.GetRequiredService<IOptions<HttpClientFactoryOptions>>().Value;

		// Assert
		Assert.Single(options.HttpMessageHandlerBuilderActions);
	}
}
