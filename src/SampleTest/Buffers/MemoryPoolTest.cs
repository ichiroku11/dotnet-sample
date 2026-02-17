using System.Buffers;

namespace SampleTest.Buffers;

public class MemoryPoolTest {
	[Fact]
	public void Rent_取得したMemoryのプロパティを確認する() {
		// Arrange
		using var owner = MemoryPool<int>.Shared.Rent();

		// Act
		var memory = owner.Memory;

		// Assert
		Assert.False(memory.IsEmpty);
		Assert.Equal(1024, memory.Length);
	}

	[Theory]
	[InlineData(1, 16)]
	[InlineData(10, 16)]
	[InlineData(17, 32)]
	[InlineData(32, 32)]
	public void Rent_最小バッファサイズを指定して取得したMemoryのLengthプロパティを確認する(int minBufferSize, int actual) {
		// Arrange
		using var owner = MemoryPool<int>.Shared.Rent(minBufferSize);

		// Act
		var memory = owner.Memory;

		// Assert
		Assert.False(memory.IsEmpty);
		Assert.Equal(actual, memory.Length);
	}
}
