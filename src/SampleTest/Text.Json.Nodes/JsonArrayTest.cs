using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SampleTest.Text.Json.Nodes;

public class JsonArrayTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void Create_生成したインスタンスのプロパティなどを確認する() {
		// Arrange

		// Act
		var jsonArray = new JsonArray(JsonValue.Create(1), JsonValue.Create(2));
		_output.WriteLine(jsonArray.ToJsonString());

		// Assert
		Assert.Null(jsonArray.Parent);
		Assert.Same(jsonArray, jsonArray.Root);
		Assert.Equal("$", jsonArray.GetPath());

		var fisrt = jsonArray[0];
		Assert.NotNull(fisrt);
		Assert.Same(jsonArray, fisrt.Parent);
		Assert.Same(jsonArray, fisrt.Root);
		Assert.Equal("$[0]", fisrt.GetPath());
		Assert.Equal(1, (int)fisrt);
		Assert.Equal(1, fisrt.GetValue<int>());

		var second = jsonArray[1];
		Assert.NotNull(second);
		Assert.Same(jsonArray, second.Parent);
		Assert.Same(jsonArray, second.Root);
		Assert.Equal("$[1]", second.GetPath());
		Assert.Equal(2, (int)second);
		Assert.Equal(2, second.GetValue<int>());
	}

	[Fact]
	public void Deserialize_JsonArrayをJsonElementに変換する() {
		// Arrange
		var jsonArray = new JsonArray(JsonValue.Create(1), JsonValue.Create(2));
		_output.WriteLine(jsonArray.ToJsonString());

		// Act
		var actual = jsonArray.Deserialize<JsonElement>();

		// Assert
		Assert.Equal(JsonValueKind.Array, actual.ValueKind);
		Assert.Equal(2, actual.GetArrayLength());

		Assert.Equal(JsonValueKind.Number, actual[0].ValueKind);
		Assert.Equal(1, actual[0].GetInt32());

		Assert.Equal(JsonValueKind.Number, actual[1].ValueKind);
		Assert.Equal(2, actual[1].GetInt32());
	}
}
