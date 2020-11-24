using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace SampleTest.Json {
	public class JsonExtensionDataAttributeTest {
		private class ExtensionDataSample {
			public int Number { get; set; }
			public string Text { get; set; }
			[JsonExtensionData]
			public Dictionary<string, object> Exts { get; set; }
		}

		[Fact]
		public void JsonExtensionData属性を使ってプロパティで表されないデータをシリアライズする() {
			// Arrange
			var data = new ExtensionDataSample {
				Number = 1,
				Text = "Abc",
				Exts = new Dictionary<string, object> {
					["other"] = "x",
				},
			};
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};

			// Act
			var actual = JsonSerializer.Serialize(data, options);

			// Assert
			var expected = @"{""number"":1,""text"":""Abc"",""other"":""x""}";
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void JsonExtensionData属性を使ってプロパティで表されないデータをデシリアライズする() {
			// Arrange
			var json = @"{""number"":1,""text"":""Abc"",""other"":""x""}";
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};

			// Act
			var data = JsonSerializer.Deserialize<ExtensionDataSample>(json, options);

			// Assert
			Assert.Equal(1, data.Number);
			Assert.Equal("Abc", data.Text);
			Assert.Single(data.Exts);
			Assert.Equal("x", data.Exts["other"].ToString());
		}
	}
}
