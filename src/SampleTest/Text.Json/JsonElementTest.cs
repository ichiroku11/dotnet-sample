using System.Text.Json;
using System.Text.Json.Nodes;

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

	[Fact]
	public void EnumerateObject_オブジェクトのプロパティを列挙できる() {
		// Arrange
		using var document = JsonDocument.Parse(@"{""x"":1,""y"":2}");
		var root = document.RootElement;

		// Act
		var values = root.EnumerateObject()
			.ToDictionary(property => property.Name, property => property.Value.GetInt32());

		// Assert
		Assert.Equal(2, values.Count);
		Assert.Equal(1, values["x"]);
		Assert.Equal(2, values["y"]);
	}

	public static TheoryData<Func<JsonElement>> GetTheoryData() {
		var values = new JsonArray(
			new JsonObject { ["x"] = JsonValue.Create(1) },
			new JsonObject { ["x"] = JsonValue.Create(2) });

		return new() {
			() => JsonSerializer.SerializeToElement(new[] { new { x = 1 }, new { x = 2 } }),
			() => JsonSerializer.Deserialize<JsonElement>(values),
			() => values.Deserialize<JsonElement>(),
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData))]
	public void オブジェクト配列のJsonElementを色々な方法で生成する(Func<JsonElement> func) {
		// Arrange

		// Act
		var actual = func();

		// Assert
		Assert.Equal(JsonValueKind.Array, actual.ValueKind);
		Assert.Equal(2, actual.GetArrayLength());
		Assert.Collection(
			actual.EnumerateArray(),
			entry => {
				Assert.Equal(JsonValueKind.Object, entry.ValueKind);
				Assert.Equal(1, entry.GetProperty("x").GetInt32());
			},
			entry => {
				Assert.Equal(JsonValueKind.Object, entry.ValueKind);
				Assert.Equal(2, entry.GetProperty("x").GetInt32());
			});
	}
}
