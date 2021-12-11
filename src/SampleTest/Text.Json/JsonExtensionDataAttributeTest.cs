using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace SampleTest.Text.Json;

// JsonExtensionData属性を使ってモデルのプロパティに含まれないデータを扱う
// 参考
// https://docs.microsoft.com/ja-jp/dotnet/standard/serialization/system-text-json-handle-overflow
// https://docs.microsoft.com/ja-jp/dotnet/api/system.text.json.serialization.jsonextensiondataattribute?view=net-5.0
public class JsonExtensionDataAttributeTest {
	private class ExtensionDataSample {
		public int Number { get; set; }
		public string Text { get; set; } = "";
		// JsonExtensionData属性を指定できるのは、
		// Dictionary<string, object>またはDictionary<string, JsonElement>
		[JsonExtensionData]
		public Dictionary<string, object> Exts { get; set; } = new();
	}

	[Fact]
	public void Serialize_JsonExtensionData属性を使ってプロパティで表されないデータをシリアライズする() {
		// Arrange
		var data = new ExtensionDataSample {
			Number = 1,
			Text = "Abc",
			Exts = new Dictionary<string, object> {
				["other1"] = "x",
				["other2"] = true,
				["other3"] = 1,
			},
		};
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		// Act
		var actual = JsonSerializer.Serialize(data, options);

		// Assert
		var expected = @"{""number"":1,""text"":""Abc"",""other1"":""x"",""other2"":true,""other3"":1}";
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Deserialize_JsonExtensionData属性を使ってプロパティで表されないデータをデシリアライズする() {
		// Arrange
		var json = @"{""number"":1,""text"":""Abc"",""other1"":""x"",""other2"":true,""other3"":1}";
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		// Act
		var data = JsonSerializer.Deserialize<ExtensionDataSample>(json, options)!;

		// Assert
		Assert.Equal(1, data.Number);
		Assert.Equal("Abc", data.Text);
		Assert.Equal(3, data.Exts.Count);

		// 型はJsonElementとなる
		var other1 = Assert.IsType<JsonElement>(data.Exts["other1"]);
		Assert.Equal("x", other1.GetString());
		var other2 = Assert.IsType<JsonElement>(data.Exts["other2"]);
		Assert.True(other2.GetBoolean());
		var other3 = Assert.IsType<JsonElement>(data.Exts["other3"]);
		Assert.Equal(1, other3.GetInt32());
	}
}
