using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Json {
	// https://docs.microsoft.com/ja-jp/dotnet/standard/serialization/system-text-json-how-to?view=netcore-3.0
	public class JsonSerializerTest {
		private readonly ITestOutputHelper _output;

		public JsonSerializerTest(ITestOutputHelper output) {
			_output = output;
		}

		[Fact]
		public void Serialize_PropertyNamingPolicyを使ってプロパティ名をキャメルケースで出力する() {
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
		public void Serialize_整形して出力する() {
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

		[Fact]
		public void Serialize_DictionaryKeyPolicyを使った違いを確認する() {
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
		public void Deserialize_ValueTupleに変換できない() {
			var (number, text) = JsonSerializer.Deserialize<(int number, string text)>(@"{""number"":1,""text"":""Abc""}");

			Assert.Equal(0, number);
			Assert.Null(text);
		}

		private class Sample {
			public int Number { get; set; }
			public string Text { get; set; }
		}

		[Fact]
		public void Deserialize_nullをパースするとArgumentNullException() {
			// Arrange
			// Act
			// Assert
			Assert.Throws<ArgumentNullException>(() => {
				JsonSerializer.Deserialize<Sample>(default(string));
			});
		}

		[Theory]
		[InlineData(typeof(Sample))]
		[InlineData(typeof(Sample[]))]
		public void Deserialize_空文字をパースするとJsonException(Type type) {
			// Arrange
			// Act
			// Assert
			Assert.Throws<JsonException>(() => {
				JsonSerializer.Deserialize("", type);
			});
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
}
