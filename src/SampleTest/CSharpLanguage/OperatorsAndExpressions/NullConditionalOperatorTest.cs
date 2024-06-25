namespace SampleTest.CSharpLanguage.OperatorsAndExpressions;

// null条件演算子のテスト
public class NullConditionalOperatorTest {
	// https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/builtin-types/nullable-value-types#how-to-identify-a-nullable-value-type

	// 指定した値がnullableかどうか
	private static bool IsNullable<TValue>(TValue _) {
		var type = typeof(TValue);
		return Nullable.GetUnderlyingType(type) != null;
	}

	private class Sample1 {
		public int Value { get; init; }
	}

	[Fact]
	public void Null条件演算子_getterプロパティの値はnull許容型になる() {
		// Arrange
		var sample = new Sample1 {
			Value = 1,
		};

		// Act
		// Assert
		Assert.Equal(1, sample.Value);

		// 通常のプロパティアクセスはnull許容型ではない
		Assert.False(IsNullable(sample.Value));

		// null条件演算子でのプロパティアクセスはnull許容型になる
		Assert.True(IsNullable(sample?.Value));
	}

	private class Sample2 {
		private int _value;
		public int this[string key] {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}
	}

	[Fact]
	public void Null条件演算子_インデクサの値はnull許容型になる() {
		// Arrange
		var sample = new Sample2 {
			["a"] = 1,
		};

		// Act
		// Assert
		Assert.Equal(1, sample["a"]);

		// 通常のインデクサアクセスはnull許容型ではない
		Assert.False(IsNullable(sample["a"]));

		// null条件演算子でのインデクサアクセスはnull許容型になる
		Assert.True(IsNullable(sample?["a"]));
	}
}
