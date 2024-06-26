namespace SampleTest.CSharpLanguage.OperatorsAndExpressions;

// 参考
// https://learn.microsoft.com/ja-jp/dotnet/csharp/language-reference/operators/member-access-operators#range-operator-
public class RangeOperatorTest {
	[Fact]
	public void Range_試してみる() {
		var numbers = "0123456789".ToCharArray();

		// すべて
		Assert.Equal("0123456789".ToCharArray(), numbers[..]);

		// 1番目からnumbers.Length-1番目
		Assert.Equal("12345678".ToCharArray(), numbers[1..^1]);

		// 最後の2つ
		Assert.Equal("89".ToCharArray(), numbers[^2..]);

		// 変数で範囲を宣言する
		var range = 2..4;
		Assert.Equal("23".ToCharArray(), numbers[range]);
	}

	// 範囲インデックス
	[Fact]
	public void Range_開始は含まれるが終了は含まれない() {
		// Arrange
		var values = new[] { 1, 2, 3, 4, 5 };

		// Act
		var actual = values[1..3];

		// Assert
		Assert.Equal(new[] { 2, 3 }, actual);
	}

	[Fact]
	public void Range_開始と終了で同じインデックスを指定すると空になる() {
		// Arrange
		var values = new[] { 1, 2, 3, 4, 5 };

		// Act
		var actual = values[2..2];

		// Assert
		Assert.Empty(actual);
	}

	[Fact]
	public void Range_開始に配列の長さを指定すると空の配列を返す() {
		// Arrange
		var values = new[] { 1, 2, 3 };

		// Act
		var actual = values[values.Length..];

		// Assert
		Assert.Empty(actual);
	}

	[Fact]
	public void Range_終了に0を指定すると空の配列を返す() {
		// Arrange
		var values = new[] { 1, 2, 3 };

		// Act
		var actual = values[..0];

		// Assert
		Assert.Empty(actual);
	}

	[Fact]
	public void Range_開始に配列の長さより大きい値を指定すると例外が発生する() {
		// Arrange
		var values = new[] { 1, 2, 3 };

		// Act
		var exception = Record.Exception(() => values[(values.Length + 1)..]);

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(exception);
	}

	[Fact]
	public void Range_終了に0より小さい値を指定すると例外が発生する() {
		// Arrange
		var values = new[] { 1, 2, 3 };

		// Act
		var exception = Record.Exception(() => values[..-1]);

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(exception);
	}

	[Fact]
	public void Range_配列の要素を最初から3つとそれ以降でわける() {
		// Arrange
		var values = new[] { 1, 2, 3, 4, 5 };

		// Act
		var actual1 = values[..3];
		var actual2 = values[3..];

		// Assert
		Assert.Equal(new[] { 1, 2, 3 }, actual1);
		Assert.Equal(new[] { 4, 5 }, actual2);
	}

	[Fact]
	public void Range_配列の要素を最後から3つとそれ以降でわける() {
		// Arrange
		var values = new[] { 1, 2, 3, 4, 5 };

		// Act
		var actual1 = values[..^3];
		var actual2 = values[^3..];

		// Assert
		Assert.Equal(new[] { 1, 2 }, actual1);
		Assert.Equal(new[] { 3, 4, 5 }, actual2);
	}
}
