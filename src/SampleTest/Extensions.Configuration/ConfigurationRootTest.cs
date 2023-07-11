using Microsoft.Extensions.Configuration;

namespace SampleTest.Extensions.Configuration;

public class ConfigurationRootTest {
	[Fact]
	public void Providers_ソースを追加しないと空になる() {
		// Arrange
		var config = new ConfigurationBuilder()
			.Build();

		// Act
		// Assert
		Assert.Empty(config.Providers);
	}

	[Fact]
	public void Providers_ソースを1つ追加する() {
		// Arrange
		var config = new ConfigurationBuilder()
			.AddInMemoryCollection()
			.Build();

		// Act
		// Assert
		Assert.Single(config.Providers);
	}
}
