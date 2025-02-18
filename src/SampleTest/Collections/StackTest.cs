namespace SampleTest.Collections;

public class StackTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void Pop_コンストラクターで指定したコレクションの逆順で取得できる() {
		// Arrange
		// Act
		var stack = new Stack<int>([1, 2, 3]);

		// Assert
		Assert.Equal(3, stack.Pop());
		Assert.Equal(2, stack.Pop());
		Assert.Equal(1, stack.Pop());
	}

	[Fact]
	public void Pop_空の状態で呼び出すとInvalidOperationExceptionが発生する() {
		// Arrange
		var stack = new Stack<string>();

		// Act
		var exception = Record.Exception(() => stack.Pop());

		// Assert
		Assert.IsType<InvalidOperationException>(exception);

		// Stack empty.
		_output.WriteLine(exception.Message);
	}

	[Fact]
	public void TryPop_空の状態で呼び出すと戻り値はfalseになりout引数はnullになる() {
		// Arrange
		var queue = new Stack<string>();

		// Act
		var actual = queue.TryPop(out var result);

		// Assert
		Assert.False(actual);
		Assert.Null(result);
	}
}
