namespace SampleTest.Linq;

public class EnumerableTest {
	[Fact]
	public void Average_Max_Min_非nullの空のシーケンスで呼び出すとInvalidOperationException() {
		// Arrange
		// Act
		var record = new {
			Average = Record.Exception(() => Enumerable.Empty<int>().Average()),
			Max = Record.Exception(() => Enumerable.Empty<int>().Max()),
			Min = Record.Exception(() => Enumerable.Empty<int>().Min()),
		};

		// Assert
		// 非nullの場合は例外
		Assert.IsType<InvalidOperationException>(record.Average);
		Assert.IsType<InvalidOperationException>(record.Max);
		Assert.IsType<InvalidOperationException>(record.Min);
	}

	[Fact]
	public void Average_Max_Min_null許容型の空のシーケンスで呼び出すとnullになる() {
		// Arrange
		// Act
		var actual = new {
			Average = Enumerable.Empty<int?>().Average(),
			Max = Enumerable.Empty<int?>().Max(),
			Min = Enumerable.Empty<int?>().Min(),
		};

		// Assert
		// null許容型の場合はnull
		Assert.Null(actual.Average);
		Assert.Null(actual.Max);
		Assert.Null(actual.Min);
	}
}
