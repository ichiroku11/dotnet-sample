using System.Collections;
using System.Text.Json;

namespace SampleTest.Text.Json;

// https://docs.microsoft.com/ja-jp/dotnet/standard/serialization/system-text-json-how-to?view=netcore-3.0
public class JsonSerializerTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Theory]
	[InlineData(1, "1")]
	[InlineData(true, "true")]
	[InlineData("Abc", "\"Abc\"")]
	public void Serialize_プリミティブ型のシリアライズ(object value, string expected) {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		// Act
		_output.WriteLine(value.GetType().ToString());
		var actual = JsonSerializer.Serialize(value, options);
		_output.WriteLine(actual);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("1", 1)]
	[InlineData("true", true)]
	[InlineData("\"Abc\"", "Abc")]
	public void Deerialize_プリミティブ型のデシリアライズ(string json, object expected) {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		// Act
		var actual = JsonSerializer.Deserialize(json, expected.GetType(), options);

		// Assert
		Assert.IsType(expected.GetType(), actual);
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Deserialize_ValueTupleに変換できない() {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true,
		};

		// Act
		var (number, text) = JsonSerializer.Deserialize<(int number, string text)>(@"{""number"":1,""text"":""Abc""}", options);

		// Assert
		Assert.Equal(0, number);
		Assert.Null(text);
	}

	private class Sample {
		public int Number { get; set; }
		public string Text { get; set; } = "";
	}

	[Fact]
	public void Deserialize_nullをパースするとArgumentNullException() {
		// Arrange
		// Act
		// Assert
		Assert.Throws<ArgumentNullException>(() => {
			JsonSerializer.Deserialize<Sample>(default(string)!);
		});
	}

	[Theory]
	[InlineData(typeof(Sample))]
	[InlineData(typeof(Sample[]))]
	public void Deserialize_空文字をパースするとJsonException(Type type) {
		// Arrange
		// Act
		// Assert
		var exception = Assert.Throws<JsonException>(() => {
			JsonSerializer.Deserialize("", type);
		});
		_output.WriteLine(exception.Message);
	}

	[Theory]
	[InlineData(typeof(Sample))]
	[InlineData(typeof(Sample[]))]
	public void Deserialize_HTML文字列をパースするとJsonException(Type type) {
		// Arrange
		var text = @"<!DOCTYPE html>
<html>
<head>
	<meta charset=""UTF-8"">
	<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
	<title></title>
</head>
<body>
</body>
</html>";

		// Act
		// Assert
		var exception = Assert.Throws<JsonException>(() => {
			JsonSerializer.Deserialize(text, type);
		});
		_output.WriteLine(exception.Message);
	}

	[Theory]
	[InlineData(typeof(Sample[]))]
	[InlineData(typeof(IEnumerable<Sample>))]
	[InlineData(typeof(List<Sample>))]
	public void Deserialize_空配列のJSON文字をパースして空のコレクションを取得できる(Type type) {
		// Arrange
		// Act
		var samples = JsonSerializer.Deserialize("[]", type) as IEnumerable;

		// Assert
		Assert.NotNull(samples);
		Assert.Empty(samples);
	}
}
