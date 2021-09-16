using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Text.Json {
	public class JsonSerializerDictionaryTest {
		private record SampleItem(string Value);

		private class SampleWithDictionary {
			public IDictionary<string, SampleItem> Items { get; init; }
		}

		[Fact]
		public void Serialize_IDictionaryはオブジェクトにシリアライズされる() {
			// Arrange
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = true,
			};

			var sample = new SampleWithDictionary {
				Items = new Dictionary<string, SampleItem>() {
					["a"] = new SampleItem("x"),
					["b"] = new SampleItem("y"),
				},
			};

			// Act
			var actual = JsonSerializer.Serialize(sample, options);

			// Assert
			// IDictionaryはオブジェクトとしてシリアライズされる
			var expected = @"{
  ""items"": {
    ""a"": {
      ""value"": ""x""
    },
    ""b"": {
      ""value"": ""y""
    }
  }
}";
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void Deserialize_オブジェクトをIDictionaryにデシリアライズする() {
			// Arrange
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};

			var json = @"{
  ""items"": {
    ""a"": {
      ""value"": ""x""
    },
    ""b"": {
      ""value"": ""y""
    }
  }
}";

			// Act
			var actual = JsonSerializer.Deserialize<SampleWithDictionary>(json, options);

			// Assert
			// オブジェクトをIDictionaryとしてデシリアライズできる
			Assert.Equal(2, actual.Items.Count);
			Assert.Equal("x", actual.Items["a"].Value);
			Assert.Equal("y", actual.Items["b"].Value);
		}

		// todo: Key/Valueペアのコレクションをデシリアライズできるか
	}
}
