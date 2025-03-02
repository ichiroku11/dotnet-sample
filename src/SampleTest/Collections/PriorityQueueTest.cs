namespace SampleTest.Collections;

public class PriorityQueueTest {
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
}
