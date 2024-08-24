namespace SampleTest.Xunit;

public class AssertTest {
	[Fact]
	public void All_すべての要素がパスすることを確認する() {
		// Arrange
		// Act
		// Assert
		Assert.All(new[] { 2, 4, 6 }, value => Assert.Equal(0, value % 2));
	}

	[Fact]
	public void NotEqual_文字列の比較は大文字小文字を区別する() {
		// Arrange
		// Act
		// Assert
		Assert.NotEqual("x", "X");
	}

	[Fact]
	public async Task ThrowsAsync_例外の発生をテストする() {
		// Arrange
		// Act
		// Assert
		var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => {
			throw new InvalidOperationException();
		});
	}

	[Fact]
	public void Contains_文字列が含まれているか検証する() {
		// Arrange
		// Act
		// Assert
		Assert.Contains("cde", "abcdefg");
	}

	[Fact]
	public void DoesNotContain_文字列が含まれていないか検証する() {
		// Arrange
		// Act
		// Assert
		Assert.DoesNotContain("fgh", "abcdefg");
	}

	[Fact]
	public void Contains_コレクションに要素が含まれているか検証する() {
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
	public void DoesNotContain_コレクションに要素が含まれていないか検証する() {
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
	public void StartsWith_文字列が指定した部分文字列で始まっているか() {
		// Arrange
		// Act
		// Assert
		Assert.StartsWith("abc", "abcdefg");
	}

	[Fact]
	public void EndsWith_文字列が指定した部分文字列で終わっているか() {
		// Arrange
		// Act
		// Assert
		Assert.EndsWith("efg", "abcdefg");
	}

	[Fact]
	public void Same_オブジェクトが同じインスタンスかどうか検証する() {
		// Arrange
		// Act
		// Assert

		// オブジェクト
		var obj = new { };
		Assert.Same(obj, obj);

		// 文字列
		Assert.Same("abc", "abc");
	}

	[Fact]
	public void NotSame() {
		// Arrange
		// Act
		// Assert
		// オブジェクト
		Assert.NotSame(new { }, new { });

		// 値型だとWarning
		// https://xunit.net/xunit.analyzers/rules/xUnit2005
		//Assert.NotSame(1, 1);
	}

	// todo: Single
	// todo: InRange/NotInRange
	// todo: Equal/StrictEqual/NotEqual/NotStrictEqual
	// todo: Equivalent
}
