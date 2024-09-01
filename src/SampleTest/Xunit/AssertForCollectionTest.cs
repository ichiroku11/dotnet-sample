namespace SampleTest.Xunit;

// コレクションに対するAssertion
public class AssertForCollectionTest {
	[Fact]
	public void All_すべての要素がパスすることを検証する() {
		// Arrange
		// Act
		// Assert
		Assert.All(new[] { 2, 4, 6 }, value => Assert.Equal(0, value % 2));
	}

	[Fact]
	public void Contains_コレクションに要素が含まれていることを検証する() {
		// Arrange
		// Act
		// Assert
		// IEnumerable<T>
		// 数値
		Assert.Contains(1, new[] { 1 });

		// IEnumerable<T>
		// 文字列
		Assert.Contains("a", new[] { "a" });

		// 条件を満たす要素が存在するか、存在しないか
		Assert.Contains(new[] { 1 }, value => value % 2 == 1);

		// HashSet<T>
		Assert.Contains(1, new HashSet<int> { 1 });

		// SortedSet<T>
		Assert.Contains(1, new SortedSet<int> { 1 });

		// Dictionary<TKey, TValue>のキー
		Assert.Contains(1, new Dictionary<int, string> { [1] = "" });
	}

	[Fact]
	public void DoesNotContain_コレクションに要素が含まれていないことを検証する() {
		// Arrange
		// Act
		// Assert
		// IEnumerable<T>
		// 数値
		Assert.DoesNotContain(2, new[] { 1 });

		// IEnumerable<T>
		// 文字列
		Assert.DoesNotContain("b", new[] { "a" });

		// 条件を満たす要素が存在するか、存在しないか
		Assert.DoesNotContain(new[] { 1 }, value => value % 2 == 0);

		// HashSet<T>
		Assert.DoesNotContain(2, new HashSet<int> { 1 });

		// SortedSet<T>
		Assert.DoesNotContain(2, new SortedSet<int> { 1 });

		// Dictionary<TKey, TValue>のキー
		Assert.DoesNotContain(2, new Dictionary<int, string> { [1] = "" });
	}

	[Fact]
	public void Distinct_同じ要素が存在しないことを検証する() {
		// Arrange
		// Act
		// Assert
		Assert.Distinct(new[] { 1, 2, 3 });
	}

	[Fact]
	public void Empty_コレクションが空であることを検証する() {
		// Arrange
		// Act
		// Assert
		Assert.Empty(Enumerable.Empty<int>());
		Assert.Empty(Array.Empty<int>());
		Assert.Empty(new List<int> { });
	}

	[Fact]
	public void NotEmpty_コレクションが空でないことを検証する() {
		// Arrange
		// Act
		// Assert
		Assert.NotEmpty(Enumerable.Repeat(1, 1));
		Assert.NotEmpty(new int[] { 1 });
		Assert.NotEmpty(new List<int> { 1 });
	}

	[Fact]
	public void Equal_コレクションが等しいことを検証する() {
		// Arrange
		// Act
		// Assert
		Assert.Equal([1, 2, 3], Enumerable.Range(1, 3));
	}

	[Fact]
	public void Equal_コレクションが等しくないことを検証する() {
		// Arrange
		// Act
		// Assert
		Assert.NotEqual([1, 2, 3], Enumerable.Range(1, 3).Reverse());
	}

	[Fact]
	public void Single_コレクションの要素が1つかであることを検証する() {
		// Arrange
		// Act
		// Assert
		// 要素が1つかどうか
		Assert.Single(new[] { 1 });

		// 条件を満たす要素が1つかどうか
		Assert.Single(new[] { 1, 2, 3 }, value => value % 2 == 0);
	}
}
