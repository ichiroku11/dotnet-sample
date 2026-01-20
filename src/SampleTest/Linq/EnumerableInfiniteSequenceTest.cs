namespace SampleTest.Linq;

public class EnumerableInfiniteSequenceTest {
	[Theory]
	[InlineData(1, 1, 3, new[] { 1, 2, 3 })]
	[InlineData(3, -1, 3, new[] { 3, 2, 1 })]
	// stepが0でもOK
	[InlineData(1, 0, 3, new[] { 1, 1, 1 })]
	public void InfiniteSequence_指定した引数からシーケンスを生成する(int start, int step, int count, int[] expected) {
		// Arrange
		// Act
		var actual = Enumerable.InfiniteSequence(start, step)
			// おそらくTakeなどが必要
			.Take(count);

		// Assert
		Assert.Equal(expected, actual);
	}
}

