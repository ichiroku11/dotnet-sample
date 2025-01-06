namespace SampleTest.Linq;

public class EnumerableCountByTest {
	// .NET 9～
	[Fact]
	public void CountBy_キーセレクターによってグルーピングした要素数を返す() {
		// Arrange
		var values = new int[] { 1, 2, 3, 4, 5 };

		// Act
		var actual = values.CountBy(value => value % 2 == 0);

		// Assert
		Assert.Equal(2, actual.Count());
		// 偶数が2個
		Assert.Equal(2, actual.Single(item => item.Key).Value);
		// 奇数が3個
		Assert.Equal(3, actual.Single(item => !item.Key).Value);
	}
}
