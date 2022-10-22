namespace SampleTest.Linq;

public class EnumerableChunkTest {

	[Fact]
	public void Chunk_指定したサイズのチャンクに分割する() {
		// Arrange
		var items = Enumerable.Range(1, 10).Reverse();

		// Act
		var chunk = items.Chunk(3);

		// Assert
		Assert.Collection(chunk,
			values => Assert.Equal(new[] { 10, 9, 8 }, values),
			values => Assert.Equal(new[] { 7, 6, 5 }, values),
			values => Assert.Equal(new[] { 4, 3, 2 }, values),
			values => Assert.Equal(new[] { 1 }, values));
	}
}
