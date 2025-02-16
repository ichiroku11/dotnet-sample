
namespace SampleTest.Collections;

public class QueueTest {
	[Fact]
	public void ToArray_Enqueueした順に格納された配列を取得できる() {
		// Arrange
		var queue = new Queue<string>();
		queue.Enqueue("a");
		queue.Enqueue("b");

		// Act
		var actual = queue.ToArray();

		// Assert
		Assert.Equal(["a", "b"], actual);
	}
}
