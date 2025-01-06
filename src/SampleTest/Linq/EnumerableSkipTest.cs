namespace SampleTest.Linq;

public class EnumerableSkipTest {
	[Fact]
	public void Skip_引数にマイナスの値を指定すると同じシーケンスが返ってくる() {
		// Arrange
		var source = new[] { 1, 2, 3 };

		// Act
		var actual = source.Skip(-1);

		// Assert
		Assert.NotSame(source, actual);
		Assert.Equal(source, actual);
	}
}
