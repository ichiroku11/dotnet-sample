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
			int? value = null;
			if (value is null) {
				Assert.Null(value);
			} else {
				AssertHelper.Fail();
			}
		}

		[Fact]
		public void Is_非nullableに変換できる() {
			int? value = 0;
			if (value is null) {
				AssertHelper.Fail();
			} else if (value is int value2) {
				// int? value => int value2
				Assert.Equal(0, value2);
			}
		}
	}
}
