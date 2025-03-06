namespace SampleTest.Collections;

public class PriorityQueueTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void Properties_生成したインスタンスのプロパティを確認する() {
		// Arrange
		var queue = new PriorityQueue<string, int>();

		// Act
		// Assert
		Assert.Equal(0, queue.Count);
		Assert.NotNull(queue.Comparer);
		Assert.NotNull(queue.UnorderedItems);
	}

	[Fact]
	public void Dequeue_コンストラクターで指定したコレクションの優先度順で取得できる() {
		// Arrange
		var queue = new PriorityQueue<string, int>([("a", 2), ("b", 1)]);

		// Act
		var actual = new string[] {
			queue.Dequeue(),
			queue.Dequeue(),
		};

		// Assert
		// 優先度順（値が小さい順）に取り出される
		Assert.Equal(["b", "a"], actual);
	}

	[Fact]
	public void Dequeue_Enqueueした順に取得できる() {
		// Arrange
		var queue = new PriorityQueue<string, int>();
		// 優先度も指定してEnqueueする
		queue.Enqueue("a", 2);
		queue.Enqueue("b", 1);

		// Act
		var actual = new string[] {
			queue.Dequeue(),
			queue.Dequeue(),
		};

		// Assert
		// 優先度順（値が小さい順）に取り出される
		Assert.Equal(["b", "a"], actual);
	}

	[Fact]
	public void Dequeue_空の状態で呼び出すとInvalidOperationExceptionが発生する() {
		// Arrange
		var queue = new PriorityQueue<string, int>();

		// Act
		var exception = Record.Exception(() => queue.Dequeue());

		// Assert
		Assert.IsType<InvalidOperationException>(exception);

		// Queue empty.
		_output.WriteLine(exception.Message);
	}
}
