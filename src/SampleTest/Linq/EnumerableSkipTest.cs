namespace SampleTest.Linq;

public class EnumerableSkipTest {
	[Theory]
	[InlineData(-1)]
	[InlineData(0)]
	public void Skip_引数にマイナスの値や0を指定すると同じシーケンスが返ってくる(int count) {
		// Arrange
		var source = new[] { 1, 2, 3 };

		// Act
		var actual = source.Skip(count);

		// Assert
		Assert.NotSame(source, actual);
		Assert.Equal(source, actual);
	}
}
