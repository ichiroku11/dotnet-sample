using System.Text.Json;
using System.Text.Json.Nodes;

namespace SampleTest.Text.Json.Nodes;

public class JsonNodeTest {
	public static TheoryData<JsonNode, string> GetTheoryData_GetPath() {
		var theoryData = new TheoryData<JsonNode, string> {
			{
				JsonValue.Create(1),
				"$"
			},
			{
				new JsonArray(JsonValue.Create(1))[0] ?? throw new InvalidOperationException(),
				"$[0]"
			},
		};

		{
			var jsonObject = new JsonObject {
				["test"] = JsonValue.Create(1),
			};

			if (jsonObject.TryGetPropertyValue("test", out var jsonNode)) {
				theoryData.Add(jsonNode!, "$.test");
			}
		}

		return theoryData;
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_GetPath))]
	public void GetPath(JsonNode node, string expected) {
		// Arrange

		// Act
		var actual = node.GetPath();

		// Assert
		Assert.Equal(expected, actual);
	}

	public static TheoryData<JsonNode, JsonValueKind> GetTheoryData_GetValueKind()
		=> new() {
			{ new JsonObject(), JsonValueKind.Object },
			{ new JsonArray(), JsonValueKind.Array },
			{ JsonValue.Create(""), JsonValueKind.String },
			{ JsonValue.Create(0), JsonValueKind.Number },
			{ JsonValue.Create(true), JsonValueKind.True },
			{ JsonValue.Create(false), JsonValueKind.False },
			// JsonValueKind.UndefinedとJsonValueKind.Nullの作り方がわからず・・
		};

	[Theory]
	[MemberData(nameof(GetTheoryData_GetValueKind))]
	public void GetValueKind(JsonNode node, JsonValueKind expected) {
		// Arrange

		// Act
		var actual = node.GetValueKind();

		// Assert
		Assert.Equal(expected, actual);
	}
}
