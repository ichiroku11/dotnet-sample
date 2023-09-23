using System.Text.Json;
using System.Text.Json.Serialization;

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

	// 必須属性を指定する
	private class SampleWithAttribute {
		public int Value { get; set; }
		[JsonRequired]
		public string Name { get; set; } = "";
	}

	// required修飾子を指定する
	private class SampleWithModifier {
		public int Value { get; set; }
		public required string Name { get; set; } = "";
	}

	private readonly ITestOutputHelper _output;

	public JsonRequiredAttributeTest(ITestOutputHelper ouput) {
		_output = ouput;
	}

	[Fact]
	public void Deserialize_プロパティがJSON文字列に存在しなくてもデシリアライズできる() {
		// Arrange
		var json = @"{""value"":1}";

		// Act
		var actual = JsonSerializer.Deserialize<Sample>(json, _options);

		// Assert
		Assert.NotNull(actual);
		Assert.Equal(1, actual.Value);
		Assert.Equal("", actual.Name);
	}

	[Fact]
	public void Deserialize_JsonRequiredAttributeを指定したプロパティがJSON文字列に存在しない場合に例外が発生する() {
		// Arrange
		var json = @"{""value"":1}";

		// Act
		var exception = Record.Exception(() => JsonSerializer.Deserialize<SampleWithAttribute>(json, _options));

		// Assert
		Assert.IsType<JsonException>(exception);

		_output.WriteLine(exception.Message);
	}

	[Fact]
	public void Deserialize_JsonRequiredAttributeを指定したプロパティがJSON文字列にnullで存在する場合はデシリアライズできる() {
		// Arrange
		var json = @"{""value"":1,""name"":null}";

		// Act
		var actual = JsonSerializer.Deserialize<SampleWithAttribute>(json, _options);

		// Assert
		Assert.NotNull(actual);
		Assert.Equal(1, actual.Value);
		Assert.Null(actual.Name);
	}

	[Fact]
	public void Deserialize_required修飾子を指定したプロパティがJSON文字列に存在しない場合に例外が発生する() {
		// Arrange
		var json = @"{""value"":1}";

		// Act
		var exception = Record.Exception(() => JsonSerializer.Deserialize<SampleWithModifier>(json, _options));

		// Assert
		Assert.IsType<JsonException>(exception);

		_output.WriteLine(exception.Message);
	}
}
