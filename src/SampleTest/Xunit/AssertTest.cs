namespace SampleTest.Xunit;

public class AssertTest {
	[Fact]
	public void IsRange_指定した値が範囲内であることを検証する() {
		// Arrange
		// Act
		// Assert
		// 境界値を含む
		Assert.InRange(actual: 0, low: 0, high: 1);
		Assert.InRange(actual: 1, low: 0, high: 1);
	}

	[Fact]
	public void IsNotRange_指定した値が範囲内でないことを検証する() {
		// Arrange
		// Act
		// Assert
		// 境界値を含まない
		Assert.NotInRange(actual: -1, low: 0, high: 1);
		Assert.NotInRange(actual: 2, low: 0, high: 1);
	}

	[Fact]
	public void Same_オブジェクトが同じインスタンスであることを検証する() {
		// Arrange
		// Act
		// Assert

		// オブジェクト
		var obj = new { };
		Assert.Same(obj, obj);

		// 文字列
		Assert.Same("abc", "abc");
	}

	[Fact]
	public void NotSame_オブジェクトが同じインスタンスでないことを検証する() {
		// Arrange
		// Act
		// Assert
		// オブジェクト
		Assert.NotSame(new { }, new { });

		// 値型だとWarning
		// https://xunit.net/xunit.analyzers/rules/xUnit2005
		//Assert.NotSame(1, 1);
	}

	[Fact]
	public async Task ThrowsAsync_例外が発生することを検証する() {
		// Arrange
		// Act
		// Assert
		var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => {
			throw new InvalidOperationException();
		});
	}

	// todo: Equal/StrictEqual/NotEqual/NotStrictEqual
	// todo: Equivalent
}
