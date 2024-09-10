using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SampleTest.Extensions.DependencyInjection;

public class OptionsBuilderBindTest {
	private class SampleOptions {
		public int Value { get; set; }
	}

	[Fact]
	public void Bind_IConfigurationをオプションにバインドする() {
		// Arrange
		var config = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?> {
				["SampleOptions:Value"] = "1"
			})
			.Build();

		// Act
		var services = new ServiceCollection();
		services
			.AddOptions<SampleOptions>()
			.Bind(config.GetSection("SampleOptions"));

		var provider = services.BuildServiceProvider();
		var options = provider.GetRequiredService<IOptions<SampleOptions>>().Value;

		// Assert
		Assert.Equal(1, options.Value);
	}
}
