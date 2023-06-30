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

	[Fact]
	public void EnumerateArray_配列の要素を列挙できる() {
		// Arrange
		using var document = JsonDocument.Parse("[1, 2, 3]");
		var root = document.RootElement;

		// Act
		var values = root.EnumerateArray()
			.Select(element => element.GetInt32());

		// Assert
		Assert.Equal(new[] { 1, 2, 3 }, values);
	}

	// todo:
	// JsonElement.EnumerateObject
}
