
namespace SampleTest.Collections;

public class QueueTest {
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
