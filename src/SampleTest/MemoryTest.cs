namespace SampleTest;

public class MemoryTest {
	[Fact]
	public void Constructor_配列を指定し生成したインスタンスのプロパティを確認する() {
		// Arrange
		var values = new[] { 1, 2, 3 };

		// Act
		var memory = new Memory<int>(values);

		// Assert
		Assert.False(memory.IsEmpty);
		Assert.Equal(3, memory.Length);
		Assert.Equal([1, 2, 3], memory.Span);
	}

	[Fact]
	public void AsMemory_配列からMemoryを取得する() {
		// Arrange
		var values = new[] { 1, 2, 3 };

		// Act
		var memory = values.AsMemory();

		// Assert
		Assert.Equal(new Memory<int>(values), memory);
	}

	// todo: MemoryPool
}
