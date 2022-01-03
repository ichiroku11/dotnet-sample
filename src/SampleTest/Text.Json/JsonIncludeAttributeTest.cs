using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace SampleTest.Text.Json;

public class JsonIncludeAttributeTest {
	// private getterはシリアライズされない
	// private getterにJsonIncludeAttributeを指定するのシリアライズされる
	private class Sample1 {
		public int Value1 { private get; set; }

		[JsonInclude]
		public int Value2 { private get; set; }
	}

	[Fact]
	public void Serialize_JsonIncludeAttributeを使ってprivateなgetterをシリアライズする() {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		// Act
		var sample = JsonSerializer.Serialize(new Sample1 { Value1 = 1, Value2 = 2 }, options);

		// Assert
		Assert.Equal(@"{""value2"":2}", sample);
	}

	// private setterはデシリアライズされない
	// private setterにJsonIncludeAttributeを指定するのデシリアライズされる
	private class Sample2 {
		public int Value1 { get; private set; }

		[JsonInclude]
		public int Value2 { get; private set; }
	}

	[Fact]
	public void Deserialize_JsonIncludeAttributeを使ってprivateなsetterをデシリアライズする() {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		// Act
		var sample = JsonSerializer.Deserialize<Sample2>(@"{""value1"":1, ""value2"":2}", options)!;

		// Assert
		Assert.Equal(0, sample.Value1);
		Assert.Equal(2, sample.Value2);
	}
}
