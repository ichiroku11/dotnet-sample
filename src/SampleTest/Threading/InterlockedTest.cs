namespace SampleTest.Threading;

public class InterlockedTest {
	[Fact]
	public void CompareExchange_Int32で値が変更されることを確認する() {
		// Arrange
		var target = 0;

		// Act
		// location1の値がcomparandと同じ場合に、
		// location1の値をvalueの値にする
		// 戻り値は元の値を返す
		var result = Interlocked.CompareExchange(location1: ref target, value: 1, comparand: 0);

		// Assert
		Assert.Equal(1, target);
		Assert.Equal(0, result);
	}
}
