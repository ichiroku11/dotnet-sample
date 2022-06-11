using System.Text.Json;

namespace SampleTest.Text.Json;

public class JsonSerializerRecordTest {
	// レコード型のサンプル
	private record SampleRecord(int Number, string Text);

	[Fact]
	public void Serialize_recordをシリアライズできる() {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		// Act
		var sample = new SampleRecord(1, "Abc");
		var json = JsonSerializer.Serialize(sample, options);

		// Assert
		Assert.Equal(@"{""number"":1,""text"":""Abc""}", json);
	}

	[Fact]
	public void Deserialize_recordに対してデシリアライズできる() {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true,
		};

		// Act
		var sample = JsonSerializer.Deserialize<SampleRecord>(@"{""number"":1,""text"":""Abc""}", options)!;

		// Assert
		Assert.Equal(1, sample.Number);
		Assert.Equal("Abc", sample.Text);
	}
}
