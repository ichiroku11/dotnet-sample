using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace SampleTest.Extensions.DependencyInjection;

public class ServiceCollectionHttpClientBuilderTest {
	private class TestHandler : DelegatingHandler {
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void AddHttpMessageHandler_HttpClientFactoryOptionsのHttpMessageHandlerBuilderActionsが追加される(bool pattern) {
		// Arrange
		var services = new ServiceCollection();
		services.ConfigureHttpClientDefaults(builder => {
			if (pattern) {
				builder.AddHttpMessageHandler(() => new TestHandler());
			} else {
				builder.AddHttpMessageHandler<TestHandler>();
			}
		});
		var provider = services.BuildServiceProvider();

		// Act
		var options = provider.GetRequiredService<IOptions<HttpClientFactoryOptions>>().Value;

		// Assert
		Assert.Single(options.HttpMessageHandlerBuilderActions);
	}

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


	[Fact]
	public void ConfigurePrimaryHttpMessageHandler_HttpClientを名前解決すると登録順にコールバックが呼ばれる() {
		// Arrange
		var called = new List<int>();
		var services = new ServiceCollection();
		services.ConfigureHttpClientDefaults(builder => {
			builder.ConfigurePrimaryHttpMessageHandler(() => {
				called.Add(1);
				return new HttpClientHandler();
			});
			builder.ConfigurePrimaryHttpMessageHandler(() => {
				called.Add(2);
				return new HttpClientHandler();
			});
		});

		var provider = services.BuildServiceProvider();

		// Act
		// Assert
		Assert.Empty(called);
		var _ = provider.GetRequiredService<HttpClient>();
		Assert.Equal([1, 2], called);
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
	public void ConfigureHttpClient_HttpClientを名前解決すると登録順にコールバックが呼ばれる() {
		// Arrange
		var called = new List<int>();
		var services = new ServiceCollection();
		services.ConfigureHttpClientDefaults(builder => {
			builder.ConfigureHttpClient(client => {
				called.Add(1);
			});
			builder.ConfigureHttpClient(client => {
				called.Add(2);
			});
		});
		var provider = services.BuildServiceProvider();

		// Act
		// Assert
		Assert.Empty(called);
		var _ = provider.GetRequiredService<HttpClient>();
		Assert.Equal([1, 2], called);
	}

	[Fact]
	public void ConfigureAdditionalHttpMessageHandlers_HttpClientFactoryOptionsのHttpMessageHandlerBuilderActionsが追加される() {
		// Arrange
		var services = new ServiceCollection();
		services.ConfigureHttpClientDefaults(builder => {
			builder.ConfigureAdditionalHttpMessageHandlers((handlers, provider) => {
			});
		});
		var provider = services.BuildServiceProvider();

		// Act
		var options = provider.GetRequiredService<IOptions<HttpClientFactoryOptions>>().Value;

		// Assert
		Assert.Single(options.HttpMessageHandlerBuilderActions);
	}

	[Fact]
	public void ConfigureAdditionalHttpMessageHandlers_HttpClientを名前解決すると登録順にコールバックが呼ばれる() {
		// Arrange
		var called = new List<int>();
		var services = new ServiceCollection();
		services.ConfigureHttpClientDefaults(builder => {
			builder.ConfigureAdditionalHttpMessageHandlers((handlers, provider) => {
				called.Add(1);
			});
			builder.ConfigureAdditionalHttpMessageHandlers((handlers, provider) => {
				called.Add(2);
			});
		});

		var provider = services.BuildServiceProvider();

		// Act
		// Assert
		Assert.Empty(called);
		var _ = provider.GetRequiredService<HttpClient>();
		Assert.Equal([1, 2], called);
	}
}
