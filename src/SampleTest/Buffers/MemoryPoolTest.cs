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
}
