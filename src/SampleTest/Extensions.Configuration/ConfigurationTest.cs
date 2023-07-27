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
	public void GetValue_boolやintを取得できる() {
		// Arrange
		var config = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?>{
				{ "t", "true" },
				{ "f", "false" },
				{ "n", "1" },
			})
			.Build();

		// Act
		// Assert
		Assert.True(config.GetValue<bool>("t"));
		Assert.False(config.GetValue<bool>("f"));
		Assert.Equal(1, config.GetValue<int>("n"));
	}

	[Fact]
	public void GetSection_サブセクションを取得できる() {
		// Arrange
		var root = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?>{
				{ "x", "1" },
			})
			.Build();

		// Act
		var section = root.GetSection("x");

		// Assert
		Assert.Equal("x", section.Key);
		Assert.Equal("x", section.Path);
		Assert.Equal("1", section.Value);
	}

	[Fact]
	public void GetSection_サブセクションがなくても戻り値はnullにならずValueがnullになる() {
		// Arrange
		var root = new ConfigurationBuilder()
			.AddInMemoryCollection()
			.Build();

		// Act
		var section = root.GetSection("x");

		// Assert
		Assert.Equal("x", section.Key);
		Assert.Equal("x", section.Path);
		Assert.Null(section.Value);
	}

	[Fact]
	public void GetRequiredSection_存在しないサブセクションを取得しようとするとInvalidOperationException() {
		// Arrange
		var config = new ConfigurationBuilder()
			.Build();

		// Act
		var exception = Record.Exception(() => config.GetRequiredSection("not-exist-key"));

		// Assert
		Assert.IsType<InvalidOperationException>(exception);
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

	[Fact]
	public void Indexer_値が上書きされることを確認する() {
		// Arrange
		var root = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?>{
				{ "x", "1" },
			})
			.AddInMemoryCollection(new Dictionary<string, string?>{
				{ "x", "2" },
			})
			.Build();

		// Act
		// Assert
		Assert.Equal("2", root["x"]);
	}
}
