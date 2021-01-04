using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Text.Json {
	public class JsonStringEnumConverterTest {
		// enum<=>文字列・数値の変換を試すテスト
		private enum SampleCode {
			Unknown = 0,
			Ok,
			NotFound,
		}

		private class SampleData {
			public SampleCode Code { get; set; }
		}

		private readonly ITestOutputHelper _output;

		public JsonStringEnumConverterTest(ITestOutputHelper output) {
			_output = output;
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
			var data = JsonSerializer.Deserialize<SampleData>(json, options);

			// Assert
			Assert.Equal(SampleCode.Ok, data.Code);
		}

		[Fact]
		public void Deerialize_デフォルトでは文字列をenumにデシリアライズできず例外が発生する() {
			// Arrange
			var json = @"{""code"":""Ok""}";
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
	}
}
