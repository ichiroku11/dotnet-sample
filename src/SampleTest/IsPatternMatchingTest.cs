using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest {
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
	}
}
