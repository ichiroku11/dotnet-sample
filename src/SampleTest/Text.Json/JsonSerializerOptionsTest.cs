using System.Text.Json;
using System.Text.Json.Serialization;

namespace SampleTest.Text.Json;

// Cache and reuse 'JsonSerializerOptions' instances
#pragma warning disable CA1869

public class JsonSerializerOptionsTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

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

	private record Sample(int Value = default, string? Text = default);

	public static TheoryData<JsonIgnoreCondition, string> GetTheoryDataForDefaultIgnoreCondition() {
		return new() {
			// デフォルト値もnullも出力される
			{ JsonIgnoreCondition.Never, @"{""value"":0,""text"":null}" },
			// デフォルト値は出力されるが、nullは出力されない
			{ JsonIgnoreCondition.WhenWritingNull, @"{""value"":0}" },
			// デフォルト値もnullも出力されない
			{ JsonIgnoreCondition.WhenWritingDefault, @"{}" },
		};
	}

	[Theory, MemberData(nameof(GetTheoryDataForDefaultIgnoreCondition))]
	public void DefaultIgnoreCondition_シリアライズするときのデフォルト値やnullの扱いを確認する(JsonIgnoreCondition condition, string expected) {
		// Arrange
		var options = new JsonSerializerOptions {
			DefaultIgnoreCondition = condition,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		// Act
		var actual = JsonSerializer.Serialize(new Sample(), options);

		Assert.Equal(expected, actual);
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

// Cache and reuse 'JsonSerializerOptions' instances
#pragma warning restore CA1869
