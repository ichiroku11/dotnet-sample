namespace SampleTest.Collections;

public class StackTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void Pop_コンストラクターで指定したコレクションの逆順で取得できる() {
		// Arrange
		var stack = new Stack<string>(["a", "b"]);

		// Act
		var actual = new string[] {
			stack.Pop(),
			stack.Pop(),
		};

		// Assert
		Assert.Equal(["b", "a"], actual);
	}

	[Fact]
	public void Pop_Pushした逆順で取得できる() {
		// Arrange
		var queue = new Stack<string>();
		queue.Push("a");
		queue.Push("b");

		// Act
		var actual = new string[] {
			queue.Pop(),
			queue.Pop(),
		};

		// Assert
		Assert.Equal(["b", "a"], actual);
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
	public void TryPop_空の状態で呼び出すと戻り値はfalseになりout引数はデフォルト値になる() {
		// Arrange
		var queue = new Stack<string>();

		// Act
		var actual = queue.TryPop(out var result);

		// Assert
		Assert.False(actual);
		Assert.Null(result);
	}
}
