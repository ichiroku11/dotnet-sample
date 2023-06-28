using System.Text.Json;

namespace SampleTest.Text.Json;

public class JsonDocumentTest {
	[Theory]
	// undefined単体はパースできない様子？
	//[InlineData("undefined", JsonValueKind.Undefined)]
	[InlineData("{}", JsonValueKind.Object)]
	[InlineData(@"{""x"":1}", JsonValueKind.Object)]
	[InlineData("[]", JsonValueKind.Array)]
	[InlineData("[1]", JsonValueKind.Array)]
	[InlineData(@"""""", JsonValueKind.String)]
	[InlineData("0", JsonValueKind.Number)]
	[InlineData("true", JsonValueKind.True)]
	[InlineData("false", JsonValueKind.False)]
	[InlineData("null", JsonValueKind.Null)]
	public void Parse_JSON文字列をパースしてValueKindを取得する(string json, JsonValueKind expected) {
		// Arrange
		// Act
		using var document = JsonDocument.Parse(json);

		// Assert
		Assert.Equal(expected, document.RootElement.ValueKind);
	}

	// todo:
	// JsonElement.EnumerateArray
	// JsonElement.EnumerateObject
}
