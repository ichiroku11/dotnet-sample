using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace SampleTest.Text.Json.Nodes;

public class JsonObjectTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void Create_生成したインスタンスのプロパティなどを確認する() {
		// Arrange

		// Act
		var jsonObject = new JsonObject {
			["test"] = JsonValue.Create(1),
		};
		_output.WriteLine(jsonObject.ToJsonString());

		// Assert
		Assert.Null(jsonObject.Parent);
		Assert.Same(jsonObject, jsonObject.Root);
		Assert.Equal("$", jsonObject.GetPath());

		var property = jsonObject["test"];
		Assert.NotNull(property);
		Assert.Same(jsonObject, property.Parent);
		Assert.Same(jsonObject, property.Root);
		Assert.Equal("$.test", property.GetPath());
		Assert.Equal(1, (int)property);
		Assert.Equal(1, property.GetValue<int>());
	}

	[Fact]
	public void Deserialize_JsonObjectをJsonElementに変換する() {
		// Arrange
		var jsonObject = new JsonObject {
			["test"] = JsonValue.Create(1),
		};
		_output.WriteLine(jsonObject.ToJsonString());

		// Act
		var actual = jsonObject.Deserialize<JsonElement>();

		// Assert
		Assert.Equal(JsonValueKind.Object, actual.ValueKind);

		Assert.Equal(JsonValueKind.Number, actual.GetProperty("test").ValueKind);
		Assert.Equal(1, actual.GetProperty("test").GetInt32());
	}
}
