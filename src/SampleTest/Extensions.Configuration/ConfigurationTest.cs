using Microsoft.Extensions.Configuration;

namespace SampleTest.Extensions.Configuration;

public class ConfigurationTest {
	[Fact]
	public void GetChildren_サブセクションは空() {
		// Arrange
		var root = new ConfigurationBuilder()
			.Build();

		// Act
		var children = root.GetChildren();

		// Assert
		Assert.Empty(children);
	}
}
