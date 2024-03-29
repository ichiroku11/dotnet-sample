using System.Text.Json;
using System.Text.Json.Serialization;

namespace SampleTest.Text.Json;

public class JsonStringEnumConverterTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	// enum<=>文字列・数値の変換を試すテスト
	private enum SampleCode {
		Unknown = 0,
		Ok,
		NotFound,
	}

	private class SampleData {
		public SampleCode Code { get; set; }
	}

	[Fact]
	public void Serialize_デフォルトではenumは数値にシリアライズされる() {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		var data = new SampleData {
			Code = SampleCode.Ok,
		};

		// Act
		var json = JsonSerializer.Serialize(data, options);

		// Assert
		Assert.Equal(@"{""code"":1}", json);
	}

	[Theory]
	// JsonStringEnumConverterを使うと、enum値を文字列でシリアライズできる
	[InlineData(false, @"{""code"":""NotFound""}")]
	// JsonStringEnumConverterにJsonNamingPolicy.CamelCaseを指定すると、enum値をキャメルケースでシリアライズできる
	[InlineData(true, @"{""code"":""notFound""}")]
	public void Serialize_JsonStringEnumConverterを使ってenumを文字列としてシリアライズする(bool camelCase, string expected) {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};
		options.Converters.Add(new JsonStringEnumConverter(camelCase ? JsonNamingPolicy.CamelCase : null));

		var data = new SampleData {
			Code = SampleCode.NotFound,
		};

		// Act
		var actual = JsonSerializer.Serialize(data, options);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Deserialize_デフォルトでは数値をenumにデシリアライズできる() {
		// Arrange
		var json = @"{""code"":1}";
		var options = new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true,
		};

		// Act
		var data = JsonSerializer.Deserialize<SampleData>(json, options)!;

		// Assert
		Assert.Equal(SampleCode.Ok, data.Code);
	}

	[Theory]
	[InlineData(@"{""code"":""Ok""}")]
	[InlineData(@"{""code"":""ok""}")]
	[InlineData(@"{""code"":""1""}")]
	public void Deerialize_デフォルトでは文字列をenumにデシリアライズできず例外が発生する(string json) {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true,
		};

		// Act
		// Assert
		var exception = Assert.Throws<JsonException>(() => {
			JsonSerializer.Deserialize<SampleData>(json, options);
		});
		_output.WriteLine(exception.ToString());
	}

	[Theory]
	[InlineData(@"{""code"":""NotFound""}")]
	// キャメルケース、すべて大文字・小文字でもデイシリアライズできる
	[InlineData(@"{""code"":""notFound""}")]
	[InlineData(@"{""code"":""notfound""}")]
	[InlineData(@"{""code"":""NOTFOUND""}")]
	// 数字、数字の文字列でもデイシリアライズできる
	[InlineData(@"{""code"":2}")]
	[InlineData(@"{""code"":""2""}")]
	public void Deerialize_JsonStringEnumConverterを使って文字列をenumにデシリアライズする(string json) {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true,
		};
		options.Converters.Add(new JsonStringEnumConverter());

		// Act
		var data = JsonSerializer.Deserialize<SampleData>(json, options)!;

		// Assert
		Assert.Equal(SampleCode.NotFound, data.Code);

	}

}
