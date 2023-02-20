namespace SampleTest.Linq;

public class EnumerableSequenceEqualTest {
	[Theory]
	[InlineData(new[] { 0 }, null)]
	[InlineData(null, new[] { 0 })]
	public void SequenceEqual_引数のどちらかがnullだとArgumentNullExceptionが発生する(IEnumerable<int> first, IEnumerable<int> second) {
		// Arrange
		// Act
		// Assert
		Assert.Throws<ArgumentNullException>(() => {
			Enumerable.SequenceEqual(first, second);
		});
	}
}
