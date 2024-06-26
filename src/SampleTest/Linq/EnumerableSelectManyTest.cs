namespace SampleTest.Linq;

public class EnumerableSelectManyTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void SelectMany_配列の配列を平坦化するサンプル() {
		// Arrange

		// Act
		var actual = new int[][] {
			[1, 2, 3],
			[4],
			[5, 6],
		}
			.SelectMany(values => values);

		// Assert
		var expected = new[] {
			1, 2, 3, 4, 5, 6
		};
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SelectMany_2重ループを平坦化で表現する() {
		// Arrange
		var source1 = Enumerable.Range(0, 3);
		var source2 = Enumerable.Range(1, 2);

		// Act
		var actual = source1.SelectMany(x => source2, (x, y) => (x, y));

		// こう書きたい
		foreach (var (x, y) in actual) {
			_output.WriteLine($"({x}, {y})");
		}
		// 以下と同じイメージ
		foreach (var x in source1) {
			foreach (var y in source2) {
				_output.WriteLine($"({x}, {y})");
			}
		}

		// Assert
		var expected = new[] {
			(0, 1), (0, 2),
			(1, 1), (1, 2),
			(2, 1), (2, 2),
		};
		Assert.Equal(expected, actual);
	}

	// 子を持つ（階層構造がある）クラス
	private class Item(string name, IEnumerable<Item>? children = default) {
		public string Name { get; } = name;

		public IEnumerable<Item> Children { get; } = children ?? [];
	}

	private static readonly IEnumerable<Item> _items = [
		new Item("1a", [
			new Item("2a", [
				new Item("3a"),
				new Item("3b"),
			]),
			new Item("2b", [
				new Item("3c"),
			]),
		]),
		new Item("1b", [
			new Item("2c", [
				new Item("3d"),
			]),
		]),
	];

	[Fact]
	public void SelectMany_あるオブジェクトのコレクションの2階層目を平坦化するサンプル() {
		// Arrange
		// Act
		var actual = _items
			.SelectMany(item => item.Children)
			.Select(item => item.Name);

		// Assert
		Assert.Equal(["2a", "2b", "2c",], actual);
	}

	[Fact]
	public void SelectMany_あるオブジェクトのコレクションの3階層目を平坦化するサンプル() {
		// Arrange
		// Act
		var actual = _items
			.SelectMany(item => item.Children)
			.SelectMany(item => item.Children)
			.Select(item => item.Name);

		// Assert
		Assert.Equal(["3a", "3b", "3c", "3d"], actual);
	}
}
