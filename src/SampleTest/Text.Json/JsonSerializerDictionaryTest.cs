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
	}
}
