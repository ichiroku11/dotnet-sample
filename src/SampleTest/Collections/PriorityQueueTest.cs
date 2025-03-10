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

	[Fact]
	public void DequeueEnqueue_Enqueueの前にDequeueする() {
		// Arrange
		var queue = new PriorityQueue<string, int>();
		queue.Enqueue("a", 2);
		queue.Enqueue("b", 1);

		// Act
		var actual = queue.DequeueEnqueue("c", 1);

		// Assert
		Assert.Equal("b", actual);
		Assert.Equal(2, queue.Count);
	}

	[Fact]
	public void DequeueEnqueue_空の状態で呼び出すとInvalidOperationExceptionが発生する() {
		// Arrange
		var queue = new PriorityQueue<string, int>();

		// Act
		var exception = Record.Exception(() => queue.DequeueEnqueue("a", 1));

		// Assert
		Assert.IsType<InvalidOperationException>(exception);

		// Queue empty.
		_output.WriteLine(exception.Message);
	}

	[Fact]
	public void EnqueueDequeue_Dequeueの前にEnqueueする() {
		// Arrange
		var queue = new PriorityQueue<string, int>();
		queue.Enqueue("a", 2);
		queue.Enqueue("b", 1);

		// Act
		// 優先度を1にすると、"b"が取り出される保証はなさそう
		var actual = queue.EnqueueDequeue("c", 3);

		// Assert
		Assert.Equal("b", actual);
		Assert.Equal(2, queue.Count);
	}

	[Fact]
	public void EnqueueDequeue_空の状態で呼び出してもEnqueueした要素を取得できて例外も発生しない() {
		// Arrange
		var queue = new PriorityQueue<string, int>();

		// Act
		var actual = queue.EnqueueDequeue("a", 1);

		// Assert
		Assert.Equal("a", actual);
		Assert.Equal(0, queue.Count);
	}
}
