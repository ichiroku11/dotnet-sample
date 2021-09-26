using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest {
	// null条件演算子のテスト
	public class NullConditionalOperatorTest {
		// https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/builtin-types/nullable-value-types#how-to-identify-a-nullable-value-type

		// 指定した値がnullableかどうか
		private static bool IsNullable<TValue>(TValue _) {
			var type = typeof(TValue);
			return Nullable.GetUnderlyingType(type) != null;
		}

		private class Sample {
			public int Value { get; init; }
		}

		[Fact]
		public void Null条件演算子_getterプロパティはnull許容型になる() {
			// Arrange
			var sample = new Sample {
				Value = 1,
			};

			// Act
			// Assert
			Assert.False(IsNullable(sample.Value));
			// null条件演算子でのアクセスはnull許容型になる
			Assert.True(IsNullable(sample?.Value));
		}
	}
}
