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

	[Fact]
	public void CompareExchange_Int32で値が変更されないことを確認する() {
		// Arrange
		var target = 0;

		// Act
		// 一致しないので値が変更されない
		var result = Interlocked.CompareExchange(location1: ref target, value: 1, comparand: 1);

		// Assert
		Assert.Equal(0, target);
		Assert.Equal(0, result);
	}

	[Fact]
	public void CompareExchange_参照型でnullと比較して値が変更されることを確認する() {
		// Arrange
		var values = default(List<int>);

		// Act
		var result = Interlocked.CompareExchange(location1: ref values, value: new List<int> { 1 }, comparand: default);

		// Assert
		Assert.NotNull(values);
		Assert.Single(values, 1);
		Assert.Null(result);
	}
}
