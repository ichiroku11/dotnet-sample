using Xunit;

namespace SampleTest;

// isを使ったパターンマッチングを試す
// https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/is
public class IsPatternMatchingTest {
	[Fact]
	public void Is_nullのチェックができる() {
		// Arrange

		// Act
		// Assert
		int? value = null;
		if (value is null) {
			Assert.Null(value);
		} else {
			AssertHelper.Fail();
		}
	}

	[Fact]
	public void Is_非nullableに変換できる() {
		// Arrange

		// Act
		// Assert
		int? value = 0;
		if (value is null) {
			AssertHelper.Fail();
		} else if (value is int value2) {
			// int? value => int value2
			Assert.Equal(0, value2);
		}
	}

	// C# 9.0
	// https://docs.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-9#pattern-matching-enhancements
	[Fact]
	public void Is_notnullのチェックができる() {
		// Arrange

		// Act
		// Assert
		int? value = null;
		if (value is not null) {
			AssertHelper.Fail();
			return;
		}

		Assert.Null(value);
	}

	[Fact]
	public void Is_andとlessthanorequalとgreaterthanorequalを使ってみる() {
		// Arrange

		// Act
		// Assert
		Assert.All(
			"0123456789".ToCharArray(),
			digit => Assert.True(digit is >= '0' and <= '9'));
	}

	// https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/operators/patterns#parenthesized-pattern
	[Theory]
	[InlineData(0, true)]
	[InlineData(1, false)]
	[InlineData(2, false)]
	[InlineData(3, true)]
	public void Is_notと括弧を使ってみる(int value, bool expected) {
		// Arrange

		// Act
		// valueは1、2以外
		var actual = value is not (1 or 2);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Is_notと括弧で間違えそうなパターンを確認する() {
		// Arrange
		// Act
		// Assert
#pragma warning disable CS8519, CS8793
		// not (1 or 2)
		// （1または2）以外であればtrue
		Assert.True(0 is not (1 or 2));
		Assert.False(1 is not (1 or 2));
		Assert.False(2 is not (1 or 2));
		Assert.True(3 is not (1 or 2));

		// not 1 or 2
		// 1以外または2であればtrue→つまり1以外であればtrue
		Assert.True(0 is not 1 or 2);
		Assert.False(1 is not 1 or 2);
		Assert.True(2 is not 1 or 2);
		Assert.True(3 is not 1 or 2);
#pragma warning restore CS8519, CS8793
	}

	// https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/operators/patterns#var-pattern
	[Theory]
	[InlineData(0, false)]
	[InlineData(1, true)]
	[InlineData(3, true)]
	[InlineData(4, false)]
	public void Is_varと使ってみる(int value, bool expected) {
		// Arrange
		static bool isInRange(Func<IEnumerable<int>> provider, int value)
			// providerは重たい処理をイメージして、
			// varでローカル変数に一度代入する
			=> provider() is var values
				&& value >= values.Min()
				&& value <= values.Max();

		// Act
		var actual = isInRange(() => Enumerable.Range(1, 3), value);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Is_プロパティパターンを試す() {
		// Arrange
		var date = new DateTime(2022, 1, 9);

		// Act
		// Assert
		Assert.True(date is { Year: 2022, Month: 1, Day: 9 });
	}
}
