using System.Text.Json;
using System.Text.Json.Serialization;

namespace SampleTest.Text.Json;

public class JsonIgnoreConditionTest {
	private class Sample {
		public int? Value { get; init; }
	}

	[Fact]
	public void WhenWritingNull_プロパティの値がnullの場合シリアライズされない() {
		// Arrange

		// Act
		var actual = JsonSerializer.Serialize(
			new Sample(),
			new JsonSerializerOptions {
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			});

		// Assert
		Assert.Equal("{}", actual);
	}

	// todo: JsonIgnoreCondition.WhenWritingDefault
}
