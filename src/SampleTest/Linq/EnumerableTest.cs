namespace SampleTest.Linq;

public class EnumerableTest {
	[Fact]
	public void Min_非nullの空のシーケンスで呼び出すとInvalidOperationException() {
		// 非nullの場合は例外
		Assert.Throws<InvalidOperationException>(() => Enumerable.Empty<int>().Min());

		// null許容型の場合はnull
		Assert.Null(Enumerable.Empty<int?>().Min());
	}

	[Fact]
	public void Max_非nullの空のシーケンスで呼び出すとInvalidOperationException() {
		// 非nullの場合は例外
		Assert.Throws<InvalidOperationException>(() => Enumerable.Empty<int>().Max());

		// null許容型の場合はnull
		Assert.Null(Enumerable.Empty<int?>().Max());
	}

	[Fact]
	public void Average_非nullの空のシーケンスで呼び出すとInvalidOperationException() {
		// 非nullの場合は例外
		Assert.Throws<InvalidOperationException>(() => Enumerable.Empty<int>().Average());

		// null許容型の場合はnull
		Assert.Null(Enumerable.Empty<int?>().Average());
	}
}
