using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Text.Json {
	public class JsonPropertyNameAttributeTest {
		private class Sample {
			// JSON文字列中のプロパティ名を指定する
			[JsonPropertyName("Val")]
			public int Value { get; init; }
		}

		[Fact]
		public void Serialize_JsonPropertyNameAttributeを使ってシリアライズする() {
			// Arrange
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};

			// Act
			var sample = JsonSerializer.Serialize(new Sample { Value = 1 }, options);

			// Assert
			// PropertyNamingPolicyよりJsonPropertyNameAttributeが優先される
			Assert.Equal(@"{""Val"":1}", sample);
		}

		[Fact]
		public void Deserialize_JsonPropertyNameAttributeを使ってデシリアライズする() {
			// Arrange
			// Act
			var sample = JsonSerializer.Deserialize<Sample>(@"{""Val"":1}");

			// Assert
			Assert.Equal(1, sample.Value);
		}
	}
}
