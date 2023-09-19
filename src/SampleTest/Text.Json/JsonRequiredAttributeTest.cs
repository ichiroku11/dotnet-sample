using System.Text.Json;

namespace SampleTest.Text.Json;

public class JsonRequiredAttributeTest {
	private static readonly JsonSerializerOptions _options
		= new() {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

	private class Sample {
		public int Value { get; set; }
		public string Name { get; set; } = "";
	}

	[Fact]
	public void Deserialize_JSON文字列に存在しなくてもデシリアライズできる() {
		// Arrange
		var json = @"{""value"":1}";

		// Act
		var actual = JsonSerializer.Deserialize<Sample>(json, _options);

		// Assert
		Assert.NotNull(actual);
		Assert.Equal(1, actual.Value);
		Assert.Equal("", actual.Name);
	}
}
