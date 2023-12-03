using System.Text.Json;
using System.Text.Json.Nodes;

namespace SampleTest.Text.Json.Nodes;

public class JsonValueTest {
	private readonly ITestOutputHelper _output;

	public JsonValueTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void Create_引数にInt32を渡して生成したインスタンスのプロパティなどを確認する() {
		// Arrange

		// Act
		var jsonValue = JsonValue.Create(1);
		_output.WriteLine(jsonValue.ToJsonString());

		// Assert
		Assert.Null(jsonValue.Parent);
		Assert.Same(jsonValue, jsonValue.Root);
		Assert.Equal(1, (int)jsonValue);
		Assert.Equal(1, jsonValue.GetValue<int>());
		Assert.Equal("$", jsonValue.GetPath());
	}

	[Fact]
	public void Deserialize_Int32のJsonValueをJsonElementに変換する() {
		// Arrange
		var jsonValue = JsonValue.Create(1);
		_output.WriteLine(jsonValue.ToJsonString());

		// Act
		var actual = jsonValue.Deserialize<JsonElement>();

		// Assert
		Assert.Equal(JsonValueKind.Number, actual.ValueKind);
		Assert.Equal(1, actual.GetInt32());
	}
}
