namespace SampleTest.CSharpLanguage.Operators;

// implicit/explicit演算子のテスト
// https://docs.microsoft.com/ja-jp/dotnet/csharp/programming-guide/statements-expressions-operators/using-conversion-operators
public class ConversionOperatorTest {
	// 0～9までの数値を表す
	private struct Digit {
		public static ITestOutputHelper? Output { get; set; }

		// すべてのDigit型をbyte型に変換できるため
		// Digit => byteは暗黙的な変換
		public static implicit operator byte(Digit value) {
			Output?.WriteLine($"implicit: {nameof(Digit)} => byte");
			return value.Value;
		}

		// すべてのbyte型をDigit型に変換できるとは限らないため
		// byte => Digitは明示的な変換
		public static explicit operator Digit(byte value) {
			Output?.WriteLine($"explicit: byte => {nameof(Digit)}");
			return new Digit(value);
		}


		public Digit(byte value) {
			if (value > 9) {
				throw new ArgumentOutOfRangeException(nameof(value));
			}
			Value = value;
		}


		public byte Value { get; }
	}

	private readonly ITestOutputHelper _output;

	public ConversionOperatorTest(ITestOutputHelper output) {
		_output = output;

		// あまりよい方法ではないが・・・
		Digit.Output = _output;
	}


	[Fact]
	public void ImplicitOperator_Digitからbyteへ暗黙的な変換ができる() {
		// Arrange
		var src = new Digit(6);

		// Act
		// キャストは不要
		byte actual = src;

		// Assert
		Assert.Equal(6, actual);
	}

	[Fact]
	public void ExplicitOperator_byteからからDigitへ暗黙的な変換ができる() {
		// Arrange
		byte src = 6;

		// Act
		var actual = (Digit)src;

		// Assert
		// 等しいのチェックはこれでいいのか？
		Assert.Equal(new Digit(6), actual);
	}

	[Fact]
	public void ExplicitOperator_キャスト時の例外を確認する() {
		// Arrange
		byte src = 10;

		// Act

		// Assert
		// コンストラクタ内でスローさせている例外
		Assert.Throws<ArgumentOutOfRangeException>(() => (Digit)src);
	}
}
