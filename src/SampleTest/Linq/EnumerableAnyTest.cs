namespace SampleTest.Linq;

public class EnumerableAnyTest {
	// https://qiita.com/whopper1962/items/53b8f07c4571b8cfb2ff
	[Fact]
	public void Any_空のコレクションの場合は条件を満たす要素が存在しないのでfalse() {
		// Arrange
		// Act
		var actual = Enumerable.Empty<int>().Any(_ => true);

		// Assert
		Assert.False(actual);
	}

	private static IEnumerable<int> Range(int start, int count, Action<int> action) {
		for (var index = start; index < count; index++) {
			action(index);
			yield return index;
		}
	}

	[Fact]
	public void Any_シーケンスの列挙を途中で中止する() {
		// Arrange
		var values = new List<int>();

		// Act
		var any = Range(0, 5, value => values.Add(value)).Any();

		// Assert
		Assert.True(any);
		Assert.Equal([0], values);
	}

	[Fact]
	public void Any_条件を満たす要素が見つかればシーケンスの列挙を途中で中止する() {
		// Arrange
		var values = new List<int>();

		// Act
		var any = Range(0, 5, value => values.Add(value)).Any(value => value >= 2);

		// Assert
		Assert.True(any);
		Assert.Equal([0, 1, 2], values);
	}

	[Fact]
	public void Any_条件を満たす要素が見つからないのでシーケンスを最後まで列挙する() {
		// Arrange
		var values = new List<int>();

		// Act
		var any = Range(0, 5, value => values.Add(value)).Any(value => value == -1);

		// Assert
		Assert.False(any);
		Assert.Equal([0, 1, 2, 3, 4], values);
	}
}
