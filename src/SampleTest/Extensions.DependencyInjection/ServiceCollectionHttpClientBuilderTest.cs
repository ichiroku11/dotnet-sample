using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace SampleTest.Extensions.DependencyInjection;

public class ServiceCollectionHttpClientBuilderTest {
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ConfigurePrimaryHttpMessageHandler_HttpClientFactoryOptionsのHttpMessageHandlerBuilderActionsに追加される(bool pattern) {
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

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	public void ConfigureHttpClient_HttpClientFactoryOptionsのHttpClientActionsが追加される(int count) {
		// Arrange
		var services = new ServiceCollection();
		services.ConfigureHttpClientDefaults(builder => {
			// ConfigureHttpClientを呼び出した回数分、HttpClientActionsに追加される
			foreach (var index in Enumerable.Range(0, count)) {
				builder.ConfigureHttpClient(client => {
				});
			}
		});
		var provider = services.BuildServiceProvider();

		// Act
		var options = provider.GetRequiredService<IOptions<HttpClientFactoryOptions>>().Value;

		// Assert
		Assert.Equal(count, options.HttpClientActions.Count);
	}

	[Fact]
	public void ConfigureHttpClient_HttpClientを名前解決するとコールバックが呼ばれる() {
		// Arrange
		var called = false;
		var services = new ServiceCollection();
		services.ConfigureHttpClientDefaults(builder => {
			builder.ConfigureHttpClient(client => {
				called = true;
			});
		});
		var provider = services.BuildServiceProvider();

		// Act
		var client = provider.GetRequiredService<HttpClient>();

		// Assert
		Assert.True(called);
	}
}
