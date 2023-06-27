using System.Text.Json;

namespace SampleTest.Text.Json;

public class JsonDocumentTest {
	[Theory]
	[InlineData("[]", JsonValueKind.Array)]
	[InlineData("[1]", JsonValueKind.Array)]
	[InlineData("{}", JsonValueKind.Object)]
	[InlineData(@"{""x"":1}", JsonValueKind.Object)]
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
