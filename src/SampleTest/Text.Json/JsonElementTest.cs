using System.Text.Json;

namespace SampleTest.Text.Json;

public class JsonElementTest {
	[Fact]
	public void GetArrayLength_配列の長さを取得する() {
		// Arrange
		using var document = JsonDocument.Parse("[1, 2, 3]");
		var element = document.RootElement;

		// Act
		var actual = element.GetArrayLength();

		// Assert
		Assert.Equal(3, actual);
	}

	// todo:
	// JsonElement.EnumerateArray
	// JsonElement.EnumerateObject
}
