namespace SampleTest.Linq;

public class EnumerableTest {
	private interface ISample {
	}

	private class Sample : ISample {
	}

	[Fact]
	public void Cast_castする() {
		// Arrange
		var values = new object[] {
			new Sample(),
			new { },
		};

		// Act
		var actual = values.Cast<ISample>();

		// Assert
		// 1つ目はcastできるので取得できる
		Assert.IsType<Sample>(actual.First());

		// 2つ目を取得しようとするとcastできないので例外が発生する
		Assert.Throws<InvalidCastException>(() => {
			actual.Skip(1).First();
		});
	}

	[Fact]
	public void OfType_castできる要素にフィルターする() {
		// Arrange
		var values = new object[] {
			new Sample(),
			new { },
		};

		// Act
		var actual = values.OfType<ISample>();

		// Assert
		// 変換できる（castできる）要素のみにフィルターされる
		var sample = Assert.Single(actual);
		Assert.IsType<Sample>(sample);
	}

	[Fact]
	public void Prepend_シーケンスの最初に要素を追加する() {
		// Arrange
		var source = new[] { 2, 3, 4 };

		// Act
		var actual = source.Prepend(1);

		// Assert
		Assert.Equal([1, 2, 3, 4], actual);
	}

	[Fact]
	public void Append_シーケンスの最後に要素を追加する() {
		// Arrange
		var source = new[] { 2, 3, 4 };

		// Act
		var actual = source.Append(5);

		// Assert
		Assert.Equal([2, 3, 4, 5], actual);
	}

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
