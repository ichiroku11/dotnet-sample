namespace SampleTest.Linq;

public class EnumerableTryGetNonEnumeratedCountTest {
	[Fact]
	public void TryGetNonEnumeratedCount_配列の場合はカウントを取得できる() {
		// Arrange
		var values = new[] { 1, 2, 3 };

		// Act
		var actualReturn = values.TryGetNonEnumeratedCount(out var actualCount);

		// Assert
		Assert.True(actualReturn);
		Assert.Equal(3, actualCount);
	}

	[Fact]
	public void TryGetNonEnumeratedCount_Enumerableの場合はカウントを取得できる() {
		// Arrange
		var values = Enumerable.Range(1, 3);

		// Act
		var actualReturn = values.TryGetNonEnumeratedCount(out var actualCount);

		// Assert
		Assert.True(actualReturn);
		Assert.Equal(3, actualCount);
	}

	[Fact]
	public void TryGetNonEnumeratedCount_yieldreturnの場合はカウントを取得できない() {
		// Arrange
		static IEnumerable<int> values(int count) {
			foreach (var value in Enumerable.Range(0, count)) {
				yield return value;
			}
		};

		// Act
		var actualReturn = values(3).TryGetNonEnumeratedCount(out var _);

		// Assert
		Assert.False(actualReturn);
	}
}
