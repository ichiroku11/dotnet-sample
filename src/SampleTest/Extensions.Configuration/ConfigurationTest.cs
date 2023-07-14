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

	[Fact]
	public void GetChildren_サブセクションが1つ() {
		// Arrange
		var root = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?>{
				{ "x", "1" },
			})
			.Build();

		// Act
		var children = root.GetChildren();

		// Assert
		var child = Assert.Single(children);
		Assert.Equal("x", child.Key);
		Assert.Equal("x", child.Path);
		Assert.Equal("1", child.Value);
	}
}
