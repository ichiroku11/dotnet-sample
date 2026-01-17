namespace SampleTest.Linq;

public class EnumerableSequenceTest {
	[Theory]
	// 2つ目の引数endInclusiveに指定した値を含む
	[InlineData(1, 5, 1, new int[] { 1, 2, 3, 4, 5 })]
	[InlineData(1, 5, 2, new int[] { 1, 3, 5 })]
	// 3つ目の引数stepによってはendInclusiveを含まない場合もある
	[InlineData(1, 5, 3, new int[] { 1, 4 })]
	[InlineData(1, 5, 5, new int[] { 1 })]
	// startとendInclusiveに同じ値を指定しても生成可能
	[InlineData(2, 2, 2, new int[] { 2 })]
	// todo: stepがマイナスもOK
	public void Sequence_指定した引数からシーケンスを生成する(int start, int endInclusive, int step, int[] expected) {
		// Arrange
		// Act
		// 2つ目の引数endInclusiveはシーケンスの最後の値（含む）を指定する
		var actual = Enumerable.Sequence(start, endInclusive, step);

		// Assert
		Assert.Equal(expected, actual);
	}
}
