namespace SampleTest.Linq;

public class EnumerableSequenceEqualTest {
	[Theory]
#pragma warning disable xUnit1012
	[InlineData(new[] { 0 }, null)]
	[InlineData(null, new[] { 0 })]
#pragma warning restore xUnit1012
	public void SequenceEqual_引数のどちらかがnullだとArgumentNullExceptionが発生する(IEnumerable<int> first, IEnumerable<int> second) {
		// Arrange
		// Act
		// Assert
		Assert.Throws<ArgumentNullException>(() => {
			Enumerable.SequenceEqual(first, second);
		});
	}

	public static TheoryData<IEnumerable<int>, IEnumerable<int>, bool> GetTheoryData_SequenceEqual()
		=> new() {
			{ [0], [0], true },
			// お互い空の場合は等しい
			{ [], [], true },
			// 順番が異なる場合は等しくない
			{ [0, 1], [1, 0], false },
		};

	[Theory]
	[MemberData(nameof(GetTheoryData_SequenceEqual))]
	public void SequenceEqual_いろいろな比較を試す(IEnumerable<int> first, IEnumerable<int> second, bool expected) {
		// Arrange
		// Act
		var actual = Enumerable.SequenceEqual(first, second);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SequenceEqual_2つのコレクションが等しいか比較できる() {
		// Arrange
		var first = new[] { 1, 2, 3 };
		var second = new[] { 1, 2, 3 };

		// Act
		// Assert
		Assert.True(first.SequenceEqual(second));
	}
}
