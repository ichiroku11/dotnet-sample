using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Json {
	public class DateTimeConverterTest {
		private readonly ITestOutputHelper _output;

		public DateTimeConverterTest(ITestOutputHelper output) {
			_output = output;
		}

		private class ConverterSample {
			public DateTime Value { get; set; }
		}

		// 独自フォーマットの日付文字列とDateTimeを変換するコンバータ
		private class DateTimeConverter : JsonConverter<DateTime> {
			private static readonly string _format = "yyyy/MM/dd HH:mm:ss";

			public override DateTime Read(
				ref Utf8JsonReader reader,
				Type typeToConvert,
				JsonSerializerOptions options)
				=> DateTime.ParseExact(reader.GetString(), _format, CultureInfo.InvariantCulture);

			public override void Write(
				Utf8JsonWriter writer,
				DateTime value,
				JsonSerializerOptions options)
				=> writer.WriteStringValue(value.ToString(_format, CultureInfo.InvariantCulture));
		}

		[Fact]
		public void デフォルトではDateTimeにデシリアライズできない独自の形式の日付文字列がある() {
			// Arrange
			var json = @"{""value"":""2020/06/01 12:34:56""}";
			var options = new JsonSerializerOptions {
				PropertyNameCaseInsensitive = true,
			};

			// Act
			// Assert
			var exception = Assert.Throws<JsonException>(() => {
				JsonSerializer.Deserialize<ConverterSample>(json, options);
			});
			_output.WriteLine(exception.ToString());
		}

		[Fact]
		public void 独自の形式の日付文字列をDateTimeにデシリアライズする() {
			// Arrange
			var json = @"{""value"":""2020/06/01 12:34:56""}";
			var options = new JsonSerializerOptions {
				PropertyNameCaseInsensitive = true,
			};
			options.Converters.Add(new DateTimeConverter());

			// Act
			var sample = JsonSerializer.Deserialize<ConverterSample>(json, options);

			// Assert
			Assert.Equal(new DateTime(2020, 6, 1, 12, 34, 56), sample.Value);
		}
	}
}
