namespace SampleTest.Linq;

public class EnumerableSequenceTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Theory]
	// 2つ目の引数endInclusiveに指定した値を含む
	[InlineData(1, 5, 1, new int[] { 1, 2, 3, 4, 5 })]
	[InlineData(1, 5, 2, new int[] { 1, 3, 5 })]
	// 3つ目の引数stepによってはendInclusiveを含まない場合もある
	[InlineData(1, 5, 3, new int[] { 1, 4 })]
	[InlineData(1, 5, 5, new int[] { 1 })]
	// startとendInclusiveに同じ値を指定しても生成可能
	[InlineData(1, 1, 1, new int[] { 1 })]
	// startとendInclusiveに同じ値を指定した場合、stepが0でもOK
	[InlineData(1, 1, 0, new int[] { 1 })]
	// stepがマイナスでもOK
	[InlineData(5, 1, -1, new int[] { 5, 4, 3, 2, 1 })]
	public void Sequence_指定した引数からシーケンスを生成する(int start, int endInclusive, int step, int[] expected) {
		// Arrange
		// Act
		// 2つ目の引数endInclusiveはシーケンスの最後の値（含む）を指定する
		var actual = Enumerable.Sequence(start, endInclusive, step);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	// 1から1ずつ増やしても-1にならない
	[InlineData(1, -1, 1)]
	// 1から0ずつ増やしても2にならない
	[InlineData(1, 2, 0)]
	public void Sequence_ArgumentOutOfRangeExceptionが発生する(int start, int endInclusive, int step) {
		// Arrange
		// Act
		var exception = Record.Exception(() => Enumerable.Sequence(start, endInclusive, step));

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(exception);
		_output.WriteLine(exception.Message);
	}
}
