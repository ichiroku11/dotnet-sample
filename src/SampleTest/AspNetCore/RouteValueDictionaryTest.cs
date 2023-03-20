using Microsoft.AspNetCore.Routing;

namespace SampleTest.AspNetCore;

public class RouteValueDictionaryTest {
	[Fact]
	public void Indexer_キーは大文字小文字を区別しない() {
		// Arrange
		var values = new RouteValueDictionary(new { Key1 = "value1", key2 = "value2" });

		// Act
		// Assert
		// 大文字のキーに対して
		Assert.Equal("value1", values["Key1"] as string);
		Assert.Equal("value1", values["key1"] as string);

		// 小文字のキーに対して
		Assert.Equal("value2", values["Key2"] as string);
		Assert.Equal("value2", values["key2"] as string);
	}

	[Fact]
	public void Indexer_存在しないキーでアクセスした場合の戻り値はnull() {
		// Arrange
		var values = new RouteValueDictionary();

		// Act
		// Assert
		Assert.Null(values["key"]);
	}

	public static TheoryData<object> GetTheoryData_Constructor() {
		return new() {
			new Dictionary<string, object?>() {
				["x"] = null,
			},
			new {
				x = null as string,
			}
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_Constructor))]
	public void Constructor_null値を格納できる(object source) {
		// Arrange
		// Act
		var values = new RouteValueDictionary(source);

		// Assert
		Assert.Single(values);
		Assert.True(values.ContainsKey("x"));
		Assert.Null(values["x"]);
	}
}
