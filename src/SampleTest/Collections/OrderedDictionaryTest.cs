
namespace SampleTest.Collections;

public class OrderedDictionaryTest {
	[Fact]
	public void GetAt_インデックス指定でKeyValuePairを取得できる() {
		// Arrange
		var dictionary = new OrderedDictionary<string, int> {
			["a"] = 1,
			["b"] = 2,
			["c"] = 3,
		};

		// Act
		var actual = new[] {
			dictionary.GetAt(0),
			dictionary.GetAt(1),
			dictionary.GetAt(2),
		};

		// Assert
		var expected = new[] {
			KeyValuePair.Create("a", 1),
			KeyValuePair.Create("b", 2),
			KeyValuePair.Create("c", 3),
		};
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void IndexOf_指定した要素のインデックスを取得できる() {
		// Arrange
		var dictionary = new OrderedDictionary<string, int> {
			["a"] = 1,
			["b"] = 2,
			["c"] = 3,
		};

		// Act
		var actual = new[] {
			dictionary.IndexOf("a"),
			dictionary.IndexOf("b"),
			dictionary.IndexOf("c"),
		};

		// Assert
		Assert.Equal([0, 1, 2], actual);
	}

	// todo: IListを継承している
	// todo: foreachで列挙できる

	// todo: SetAt
	// todo: RemoveAt
	// todo: InsertAt
}
