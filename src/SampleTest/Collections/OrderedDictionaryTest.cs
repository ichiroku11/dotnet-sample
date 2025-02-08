namespace SampleTest.Collections;

public class OrderedDictionaryTest {
	[Fact]
	public void OrderedDictionaryはIListを実装している() {
		// Arrange
		var dictionary = new OrderedDictionary<string, int> {
		};

		// Act
		// Assert
		Assert.IsAssignableFrom<IList<KeyValuePair<string, int>>>(dictionary);
	}

	[Fact]
	public void OrderedDictionaryはインデックスの順番にforeachで列挙できる() {
		// Arrange
		var dictionary = new OrderedDictionary<string, int> {
			["a"] = 1,
			["b"] = 2,
			["c"] = 3,
		};

		// Act
		var actual = new List<KeyValuePair<string, int>>();
		foreach (var item in dictionary) {
			actual.Add(item);
		}

		// Assert
		var expected = new[] {
			KeyValuePair.Create("a", 1),
			KeyValuePair.Create("b", 2),
			KeyValuePair.Create("c", 3),
		};
		Assert.Equal(expected, actual);
	}

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
	public void SetAt_インデックス指定でKeyValuePairを設定できる() {
		// Arrange
		var dictionary = new OrderedDictionary<string, int> {
			["a"] = 1,
			["b"] = 2,
			["c"] = 3,
		};

		// Act
		// ["b"] = 2 が ["d"] = 4 に変更する
		dictionary.SetAt(1, "d", 4);

		// Assert
		var expected = new[] {
			KeyValuePair.Create("a", 1),
			// ["b"] = 2 が ["d"] = 4 に変更されている
			KeyValuePair.Create("d", 4),
			KeyValuePair.Create("c", 3),
		};
		Assert.Equal(expected, dictionary);
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

	[Fact]
	public void IndexOf_指定した要素が見つからない場合() {
		// Arrange
		var dictionary = new OrderedDictionary<string, int> {
		};

		// Act
		var actual = dictionary.IndexOf("");

		// Assert
		Assert.Equal(-1, actual);
	}

	[Fact]
	public void Insert_インデックス指定で要素を挿入できる() {
		// Arrange
		var dictionary = new OrderedDictionary<string, int> {
			["a"] = 1,
			["c"] = 3,
		};

		// Act
		dictionary.Insert(1, "b", 2);

		// Assert
		var expected = new[] {
			KeyValuePair.Create("a", 1),
			KeyValuePair.Create("b", 2),
			KeyValuePair.Create("c", 3),
		};
		Assert.Equal(expected, dictionary);
	}

	[Fact]
	public void RemoveAt_インデックス指定で削除できる() {
		// Arrange
		var dictionary = new OrderedDictionary<string, int> {
			["a"] = 1,
			["b"] = 2,
			["c"] = 3,
		};

		// Act
		dictionary.RemoveAt(1);

		// Assert
		var expected = new[] {
			KeyValuePair.Create("a", 1),
			KeyValuePair.Create("c", 3),
		};
		Assert.Equal(expected, dictionary);
	}
}
