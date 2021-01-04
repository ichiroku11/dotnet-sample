using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Text.Json {
	public class JsonStringEnumConverterTest {
		// enum<=>文字列・数値の変換を試すテスト
		private enum SampleCode {
			Unknown = 0,
			Ok,
			NotFound,
		}

		private class Sample {
			public SampleCode Code { get; set; }
		}

		[Fact]
		public void デフォルトではenumは数値にシリアライズされる() {
			// Arrange
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};

			var data = new Sample {
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
		public void JsonStringEnumConverterを使ってenumを文字列としてシリアライズする(bool camelCase, string expected) {
			// Arrange
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};
			options.Converters.Add(new JsonStringEnumConverter(camelCase ? JsonNamingPolicy.CamelCase : null));

			var data = new Sample {
				Code = SampleCode.NotFound,
			};

			// Act
			var actual = JsonSerializer.Serialize(data, options);

			// Assert
			Assert.Equal(expected, actual);
		}
	}
}
