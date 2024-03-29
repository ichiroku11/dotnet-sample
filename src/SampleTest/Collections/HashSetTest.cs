namespace SampleTest.Collections;

public class HashSetTest {
	private class Sample {
		public int Id { get; set; }
		public string Name { get; set; } = "";
	}

	[Fact]
	public void Contains_参照型の同じインスタンスならプロパティの値が変わっても含まれる() {
		// Arrange
		var sample = new Sample { Id = 1, Name = "1" };
		var set = new HashSet<Sample> { sample };

		// Act
		var containsBefore = set.Contains(sample);

		// 同じインスタンスであればプロパティの値が変わっても含まれる
		sample.Id = 2;
		sample.Name = "2";
		var containsAfter = set.Contains(sample);

		// Assert
		Assert.True(containsBefore);
		Assert.True(containsAfter);
	}

	[Fact]
	public void Contains_参照型の異なるインスタンスならプロパティが同じでも含まれない() {
		// Arrange
		var sample = new Sample { Id = 1, Name = "1" };
		var set = new HashSet<Sample> { sample };

		// Act
		// 異なるインスタンスは含まれない
		var contains = set.Contains(new Sample { Id = 1, Name = "1" });

		// Assert
		Assert.False(contains);
	}

	[Theory]
	[InlineData(new[] { 0, 1, 2 }, true)]
	[InlineData(new[] { 0, 1 }, false)]
	[InlineData(new[] { 0, 1, 2, 3 }, true)]
	public void IsSubsetOf_otherの部分集合かどうかを判定する(IEnumerable<int> other, bool expected) {
		// Arrange
		var set = new HashSet<int> { 0, 1, 2 };

		// Act
		// otherの部分集合（＝サブセット）かどうか
		var actual = set.IsSubsetOf(other);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	// 真部分集合ではない
	[InlineData(new[] { 0, 1, 2 }, false)]
	[InlineData(new[] { 0, 1 }, false)]
	// 真部分集合である
	[InlineData(new[] { 0, 1, 2, 3 }, true)]
	public void IsProperSubsetOf_otherの真部分集合かどうかを判定する(IEnumerable<int> other, bool expected) {
		// Arrange
		var set = new HashSet<int> { 0, 1, 2 };

		// Act
		// otherの真部分集合かどうか
		var actual = set.IsProperSubsetOf(other);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new[] { 0, 1, 2 }, true)]
	[InlineData(new[] { 0, 1 }, true)]
	[InlineData(new[] { 0, 1, 2, 3 }, false)]
	public void IsSupersetOf_otherの上位集合かどうかを判定する(IEnumerable<int> other, bool expected) {
		// Arrange
		var set = new HashSet<int> { 0, 1, 2 };

		// Act
		// otherの上位集合（＝スーパーセット）かどうか
		var actual = set.IsSupersetOf(other);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new[] { 0, 1, 2 }, false)]
	[InlineData(new[] { 0, 1 }, true)]
	[InlineData(new[] { 0, 1, 2, 3 }, false)]
	public void IsProperSupersetOf_otherの真上位集合かどうかを判定する(IEnumerable<int> other, bool expected) {
		// Arrange
		var set = new HashSet<int> { 0, 1, 2 };

		// Act
		// otherの真上位集合かどうか
		var actual = set.IsProperSupersetOf(other);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new[] { 1 }, true)]
	[InlineData(new[] { 0, 1 }, true)]
	[InlineData(new[] { 0, 3 }, true)]
	[InlineData(new[] { 3 }, false)]
	public void Overlaps_otherと共通の要素が存在するかを判定する(IEnumerable<int> other, bool expected) {
		// Arrange
		var set = new HashSet<int> { 0, 1, 2 };

		// Act
		var actual = set.Overlaps(other);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	// まったく同じ要素
	[InlineData(new[] { 0, 1, 2 }, true)]
	// otherの要素が足りない
	[InlineData(new[] { 0, 1 }, false)]
	// otherの要素が多い
	[InlineData(new[] { 0, 1, 2, 3 }, false)]
	public void SetEquals_otherが同じ要素を含んでいるかどうかを判定する(IEnumerable<int> other, bool expected) {
		// Arrange
		var set = new HashSet<int> { 0, 1, 2 };

		// Act
		var actual = set.SetEquals(other);

		// Assert
		Assert.Equal(expected, actual);
	}
}
