namespace SampleTest;

public class ReadOnlyMemoryTest {
	[Fact]
	public void Constructor_配列を指定し生成したインスタンスのプロパティを確認する() {
		// Arrange
		var values = new[] { 1, 2, 3 };

		// Act
		var memory = new ReadOnlyMemory<int>(values);

		// Assert
		Assert.False(memory.IsEmpty);
		Assert.Equal(3, memory.Length);
		Assert.Equal([1, 2, 3], memory.Span);
	}

	[Fact]
	public void AsMemory_文字列からReadOnlyMemoryを取得する() {
		// Arrange

		// Act
		// 文字列の場合は、Memory<char>ではなくReadOnlyMemory<char>に変換されるメソッドのみが提供されている様子
		var memory = "abc".AsMemory();

		// Assert
		Assert.Equal(new ReadOnlyMemory<char>(['a', 'b', 'c']), memory);
	}
}
