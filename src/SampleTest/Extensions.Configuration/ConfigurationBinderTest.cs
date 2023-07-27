using Microsoft.Extensions.Configuration;

namespace SampleTest.Extensions.Configuration;

public class ConfigurationBinderTest {

	public class SampleOptions {
		public int Value { get; init; }
	}

	[Fact]
	public void Bind_複合型にバインドする() {
		// Arrange
		var root = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?> {
				{ "x:value", "1" },
			})
			.Build();
		var options = new SampleOptions();

		// Act
		root.GetSection("x").Bind(options);

		// Assert
		Assert.Equal(1, options.Value);
	}

	[Fact]
	public void Get_複合型として取得する() {
		// Arrange
		var root = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?> {
				{ "x:value", "1" },
			})
		.Build();

		// Act
		var options = root.GetSection("x").Get<SampleOptions>();

		// Assert
		Assert.NotNull(options);
		Assert.Equal(1, options.Value);
	}
}
