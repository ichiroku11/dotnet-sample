using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Text.Json {
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

		// init専用セッターのサンプル
		private class SampleWithInitOnlySetter {
			public int Number { get; init; }
			public string Text { get; init; }
		}

		[Fact]
		public void Deserialize_init専用セッターのプロパティに対してデシリアライズできる() {
			// Arrange
			var options = new JsonSerializerOptions {
				PropertyNameCaseInsensitive = true,
			};

			// Act
			var sample = JsonSerializer.Deserialize<SampleWithInitOnlySetter>(@"{""number"":1,""text"":""Abc""}", options);

			// Assert
			Assert.Equal(1, sample.Number);
			Assert.Equal("Abc", sample.Text);
		}

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
			var sample = JsonSerializer.Deserialize<SampleRecord>(@"{""number"":1,""text"":""Abc""}", options);

			// Assert
			Assert.Equal(1, sample.Number);
			Assert.Equal("Abc", sample.Text);
		}
	}
}
