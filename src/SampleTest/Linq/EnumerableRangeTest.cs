namespace SampleTest.Linq;

public class EnumerableRangeTest {
	[Fact]
	public void Range_指定した範囲内の整数のシーケンスを生成する() {
		// Arrange
		// Act
		// 2つ目の引数countは個数を指定する
		var actual = Enumerable.Range(start: 1, count: 5);

		// Assert
		Assert.Equal([1, 2, 3, 4, 5], actual);
	}
}
