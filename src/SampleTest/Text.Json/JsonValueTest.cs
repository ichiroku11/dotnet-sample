using System.Text.Json;
using System.Text.Json.Nodes;

namespace SampleTest.Text.Json;

public class JsonValueTest {
	private readonly ITestOutputHelper _output;

	public JsonValueTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void Create_引数にInt32を渡して生成したインスタンスのプロパティなどを確認する() {
		// Arrange

		// Act
		var actual = JsonValue.Create(1);
		_output.WriteLine(actual.ToJsonString());

		// Assert
		Assert.Null(actual.Parent);
		Assert.Same(actual, actual.Root);
		Assert.Equal(1, (int)actual);
		Assert.Equal(1, actual.GetValue<int>());
		Assert.Equal("$", actual.GetPath());
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
