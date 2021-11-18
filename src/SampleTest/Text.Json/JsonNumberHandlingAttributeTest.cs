using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Text.Json;

public class JsonNumberHandlingAttributeTest {
	private class Sample {
		public int Value1 { get; init; }

		[JsonNumberHandling(
			// 文字列の数値を読み取る
			JsonNumberHandling.AllowReadingFromString |
			// 文字列として書き込む
			JsonNumberHandling.WriteAsString)]
		public int Value2 { get; init; }
	}

	[Fact]
	public void Deserialize_文字列を数値にデシリアライズしようとするJsonExceptionが発生する() {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true,
		};
		var json = @"{""value1"":""1""}";

		// Act
		// Assert
		Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Sample>(json, options));
	}

	[Fact]
	public void Deserialize_数値文字列をデシリアライズする() {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true,
		};

		// value2の値は数値文字列
		var json = @"{""value1"":1,""value2"":""2""}";

		// Act
		var sample = JsonSerializer.Deserialize<Sample>(json, options);

		// Assert
		Assert.Equal(1, sample.Value1);
		Assert.Equal(2, sample.Value2);
	}

	[Fact]
	public void Serialize_数値を文字列としてシリアライズする() {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		// Act
		var json = JsonSerializer.Serialize(new Sample { Value1 = 1, Value2 = 2 }, options);

		// Assert
		// value2の値は文字列
		Assert.Equal(@"{""value1"":1,""value2"":""2""}", json);
	}
}
