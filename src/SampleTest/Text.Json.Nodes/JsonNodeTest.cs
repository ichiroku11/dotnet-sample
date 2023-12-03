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
}
