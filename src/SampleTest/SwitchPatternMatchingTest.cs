using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest {
	// switchを使ったパターンマッチングを試す
	// https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/switch
	public class SwitchPatternMatchingTest {
		[Fact]
		public void Switch_nullのチェックができる() {
			int? value = null;
			switch (value) {
				case null:
					Assert.Null(value);
					break;
				case int value2:
					AssertHelper.Fail();
					break;
			}
		}

		[Fact]
		public void Switch_非nullableに変換できる() {
			int? value = 0;
			switch (value) {
				case null:
					AssertHelper.Fail();
					break;
				case int value2:
					Assert.Equal(0, value2);
					break;
			}
		}

		[Fact]
		public void Switch_when句を使ったサンプル1() {
			int? value = 1;
			switch (value) {
				case int value2 when value2 % 2 == 1:
					Assert.Equal(1, value2);
					break;
				case int value2 when value2 % 2 == 0:
					// when句の条件が成り立たないので呼ばれない
					AssertHelper.Fail();
					break;
			}
		}

		[Fact]
		public void Switch_when句を使ったサンプル2() {
			int? value = 2;
			switch (value) {
				case int value2 when value2 % 2 == 1:
					// when句の条件が成り立たないので呼ばれない
					AssertHelper.Fail();
					break;
				case int value2 when value2 % 2 == 0:
					Assert.Equal(2, value2);
					break;
			}
		}
	}
}
