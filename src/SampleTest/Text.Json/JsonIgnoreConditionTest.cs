using System.Text.Json;
using System.Text.Json.Serialization;

namespace SampleTest.Text.Json;

public class JsonIgnoreConditionTest {
	private class SampleNull {
		public int? Value { get; init; }
	}

	[Fact]
	public void WhenWritingNull_プロパティの値がnullの場合はシリアライズされない() {
		// Arrange

		// Act
		var actual = JsonSerializer.Serialize(
			new SampleNull(),
			new JsonSerializerOptions {
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			});

		// Assert
		Assert.Equal("{}", actual);
	}

	private class SampleDefault {
		public int Value { get; init; }
	}

	[Fact]
	public void WhenWritingDefault_プロパティの値がデフォルトの場合はシリアライズされない() {
		// Arrange

		// Act
		var actual = JsonSerializer.Serialize(
			new SampleDefault(),
			new JsonSerializerOptions {
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
			});

		// Assert
		Assert.Equal("{}", actual);
	}
}
