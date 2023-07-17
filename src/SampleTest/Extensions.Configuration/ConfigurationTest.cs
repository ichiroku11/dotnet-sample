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
	public void GetChildren_サブセクションが1つある() {
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

	[Fact]
	public void GetChildren_サブセクションが2つある() {
		// Arrange
		var root = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?>{
				{ "x", "1" },
				{ "y", "2" },
			})
			.Build();

		// Act
		var children = root.GetChildren();

		// Assert
		Assert.Collection(
			children.OrderBy(child => child.Key),
			child => {
				Assert.Equal("x", child.Key);
				Assert.Equal("x", child.Path);
				Assert.Equal("1", child.Value);
			},
			child => {
				Assert.Equal("y", child.Key);
				Assert.Equal("y", child.Path);
				Assert.Equal("2", child.Value);
			});
	}

	[Fact]
	public void Indexer_サブセクションの値を取得できる() {
		// Arrange
		var root = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?>{
				{ "x", "1" },
			})
			.Build();

		// Act
		// Assert
		Assert.Equal("1", root["x"]);
	}
}
