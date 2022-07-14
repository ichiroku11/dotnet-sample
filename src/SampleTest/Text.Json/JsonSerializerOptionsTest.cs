using System.Text.Json;
using System.Text.Json.Serialization;

namespace SampleTest.Text.Json;

public class JsonSerializerOptionsTest {
	private readonly ITestOutputHelper _output;

	public JsonSerializerOptionsTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void DefaultIgnoreCondition_Alwaysを設定するとArgumentExceptionがスローされる() {
		// Arrange
		// Act
		// Assert
		Assert.Throws<ArgumentException>(() => {
			var options = new JsonSerializerOptions {
				DefaultIgnoreCondition = JsonIgnoreCondition.Always
			};
		});
	}

	[Fact]
	public void DictionaryKeyPolicy_違いを確認する() {
		var model = new {
			Number = 1,
			Items = new Dictionary<string, int> {
				{ "Key1", 10 },
			},
		};

		// DictionaryKeyPolicyを指定しないとディクショナリオブジェクトのプロパティ名が大文字になる
		{
			var options = new JsonSerializerOptions {
				//DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = true
			};

			var actual = JsonSerializer.Serialize(model, options);
			_output.WriteLine(actual);

			var expected = @"{
  ""number"": 1,
  ""items"": {
    ""Key1"": 10
  }
}";
			Assert.Equal(expected, actual);
		}

		// DictionaryKeyPolicyを指定してディクショナリオブジェクトのプロパティ名を小文字にする
		{
			var options = new JsonSerializerOptions {
				DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = true
			};

			var actual = JsonSerializer.Serialize(model, options);
			_output.WriteLine(actual);

			var expected = @"{
  ""number"": 1,
  ""items"": {
    ""key1"": 10
  }
}";
			Assert.Equal(expected, actual);
		}
	}

	[Fact]
	public void PropertyNamingPolicy_を使ってプロパティ名をキャメルケースでシリアライズする() {
		// Arrange
		var model = new { Number = 1, Text = "Abc" };
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		// Act
		var actual = JsonSerializer.Serialize(model, options);
		_output.WriteLine(actual);

		// Assert
		var expected = @"{""number"":1,""text"":""Abc""}";
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void WriteIndented_整形してシリアライズする() {
		// Arrange
		var model = new { Number = 1, Text = "Abc" };
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		};

		// Act
		var actual = JsonSerializer.Serialize(model, options);
		_output.WriteLine(actual);

		// Assert
		var expected = @"{
  ""number"": 1,
  ""text"": ""Abc""
}";
		Assert.Equal(expected, actual);
	}
}
