
namespace SampleTest.Collections;

public class QueueTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void Enqueue_Dequeue_Enqueueした順にDequeueできる() {
		// Arrange
		var queue = new Queue<string>();
		queue.Enqueue("a");
		queue.Enqueue("b");

		// Act
		var actual = new string[] {
			queue.Dequeue(),
			queue.Dequeue(),
		};

		// Assert
		Assert.Equal(["a", "b"], actual);
	}

	[Fact]
	public void Dequeue_空の状態で呼び出すとInvalidOperationExceptionが発生する() {
		// Arrange
		var queue = new Queue<string>();

		// Act
		var exception = Record.Exception(() => queue.Dequeue());

		// Assert
		Assert.IsType<InvalidOperationException>(exception);

		// Queue empty.
		_output.WriteLine(exception.Message);
	}

	[Fact]
	public void TryDequeue_空の状態で呼び出すと戻り値はfalseになりout引数はnullになる() {
		// Arrange
		var queue = new Queue<string>();

		// Act
		var actual = queue.TryDequeue(out var result);

		// Assert
		Assert.False(actual);
		Assert.Null(result);
	}

	[Fact]
	public void ToArray_Enqueueした順に格納された配列を取得できる() {
		// Arrange
		var queue = new Queue<string>();
		queue.Enqueue("a");
		queue.Enqueue("b");
		queue.Dequeue();
		queue.Enqueue("a");

		// Act
		var actual = queue.ToArray();

		// Assert
		Assert.Equal(["b", "a"], actual);
	}
}
