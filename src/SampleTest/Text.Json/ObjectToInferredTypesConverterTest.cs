using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Text.Json {
	public class ObjectToInferredTypesConverterTest {
		// 型を推論してデシリアライズするJsonConverter
		// 参考
		// https://docs.microsoft.com/ja-jp/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-5-0#deserialize-inferred-types-to-object-properties
		private class ObjectToInferredTypesConverter : JsonConverter<object> {
			public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
				=> reader.TokenType switch {
					// bool
					JsonTokenType.True => true,
					JsonTokenType.False => false,
					// long
					JsonTokenType.Number when reader.TryGetInt64(out var value) => value,
					// double
					JsonTokenType.Number => reader.GetDouble(),
					// DateTime
					JsonTokenType.String when reader.TryGetDateTime(out DateTime datetime) => datetime,
					// string
					JsonTokenType.String => reader.GetString(),
					// 上記以外（たぶんオブジェクトとか配列とか）
					_ => JsonDocument.ParseValue(ref reader).RootElement.Clone()
				};

			public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options) {
				throw new NotImplementedException();
			}
		}

		private static JsonSerializerOptions GetJsonSerializerOptions() {
			var options = new JsonSerializerOptions {
				PropertyNameCaseInsensitive = true,
			};
			options.Converters.Add(new ObjectToInferredTypesConverter());

			return options;
		}

		public static IEnumerable<object[]> GetTestDataForDeserialize() {
			// bool
			yield return new object[] {
				@"{""value"":true}",
				true
			};

			// long
			yield return new object[] {
				@"{""value"":1}",
				1L
			};

			// string
			yield return new object[] {
				@"{""value"":""abc""}",
				"abc"
			};

			// DateTime
			yield return new object[] {
				@"{""value"":""2021-01-09""}",
				new DateTime(2021, 1, 9),
			};
		}

		private class SampleData1 {
			public object Value { get; init; }
		}

		[Theory]
		[MemberData(nameof(GetTestDataForDeserialize))]
		public void Deserialize_推論された型をobjectのプロパティにデシリアライズする(string json, object expected) {
			// Arrange

			// Act
			var data = JsonSerializer.Deserialize<SampleData1>(json, GetJsonSerializerOptions());

			// Assert
			Assert.IsType(expected.GetType(), data.Value);
			Assert.Equal(expected, data.Value);
		}

		private class SampleData2 {
			// プロパティに定義されていないデータを扱う
			[JsonExtensionData]
			public Dictionary<string, object> Exts { get; init; }
		}

		[Theory]
		[MemberData(nameof(GetTestDataForDeserialize))]
		public void Deserialize_推論された型をJsonExtensionDataを指定したobjectのディクショナリにデシリアライズする(string json, object expected) {
			// Arrange

			// Act
			var data = JsonSerializer.Deserialize<SampleData2>(json, GetJsonSerializerOptions());

			// Assert
			Assert.Single(data.Exts);
			Assert.IsType(expected.GetType(), data.Exts["value"]);
			Assert.Equal(expected, data.Exts["value"]);
		}

		[Theory]
		[MemberData(nameof(GetTestDataForDeserialize))]
		public void Deserialize_推論された型をobjectのディクショナリにデシリアライズする(string json, object expected) {
			// Arrange

			// Act
			var values = JsonSerializer.Deserialize<Dictionary<string, object>>(json, GetJsonSerializerOptions());

			// Assert
			Assert.Single(values);
			Assert.IsType(expected.GetType(), values["value"]);
			Assert.Equal(expected, values["value"]);
		}
	}
}
