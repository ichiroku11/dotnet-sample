using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Text.Json {
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
		public void Serialize_数値を文字列としてシリアライズする() {
			// Arrange
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};

			// Act
			var json = JsonSerializer.Serialize(new Sample { Value1 = 1, Value2 = 1 }, options);

			// Assert
			Assert.Equal(@"{""value1"":1,""value2"":""1""}", json);
		}
	}
}
