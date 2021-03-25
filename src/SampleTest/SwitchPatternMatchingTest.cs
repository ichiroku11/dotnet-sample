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
				case int:
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

		[Theory]
		[InlineData(null, null, 1)]
		[InlineData(1, null, 2)]
		[InlineData(null, 2, 3)]
		[InlineData(1, 2, 4)]
		public void Switch_タプルと組み合わせて使うサンプル(int? value1, int? value2, int expected) {
			// switch式
			var actual = (value1, value2) switch {
				(null, null) => 1,
				(_, null) => 2,
				(null, _) => 3,
				_ => 4,
			};
			Assert.Equal(expected, actual);

			// switch文で書くと
			switch (value1, value2) {
				case (null, null): actual = 1; break;
				case (_, null): actual = 2; break;
				case (null, _): actual = 3; break;
				default: actual = 4; break;
			}
			Assert.Equal(expected, actual);
		}

		// タプルとswitch式を使ったうるう年の判定
		private static bool IsLeapYear(int year)
			=> (year % 400, year % 100, year % 4) switch {
				(0, _, _) => true,  // 400の倍数 => うるう年
				(_, 0, _) => false, // ↑以外で100の倍数 => 平年
				(_, _, 0) => true,  // ↑以外で4の倍数 => うるう年
				_ => false,
			};

		[Theory]
		[InlineData(2000, true)]
		[InlineData(2020, true)]
		[InlineData(2021, false)]
		[InlineData(2022, false)]
		[InlineData(2023, false)]
		[InlineData(2024, true)]
		[InlineData(2100, false)]
		public void IsLeapYear_正しく判定できる(int year, bool expected) {
			// Arrange
			// Act
			var actual = IsLeapYear(year);

			// Assert
			Assert.Equal(expected, actual);
		}


		public class Point {
			public Point(int x = 0, int y = 0) => (X, Y) = (x, y);
			public int X { get; }
			public int Y { get; }
			public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
		}

		public enum Quadrant {
			Unknown = -1,
			Origin = 0,
			One,
			Two,
			Three,
			Four,
			OnBorder,
		}

		public static IEnumerable<object[]> GetTestData() {
			yield return new object[] { new Point(0, 0), Quadrant.Origin };
			yield return new object[] { new Point(1, 1), Quadrant.One };
			yield return new object[] { new Point(-1, 1), Quadrant.Two };
			yield return new object[] { new Point(-1, -1), Quadrant.Three };
			yield return new object[] { new Point(1, -1), Quadrant.Four };
			yield return new object[] { new Point(0, 1), Quadrant.OnBorder };
			yield return new object[] { new Point(1, 0), Quadrant.OnBorder };
		}

		[Theory]
		[MemberData(nameof(GetTestData))]
		public void Switch_Deconstructを利用したサンプル(Point point, Quadrant expected) {
			// Arrange
			// Act
			var actual = point switch {
				(0, 0) => Quadrant.Origin,
				(_, 0) => Quadrant.OnBorder,
				(0, _) => Quadrant.OnBorder,
				var (x, y) when x > 0 && y > 0 => Quadrant.One,
				var (x, y) when x < 0 && y > 0 => Quadrant.Two,
				var (x, y) when x < 0 && y < 0 => Quadrant.Three,
				var (x, y) when x > 0 && y < 0 => Quadrant.Four,
				_ => throw new InvalidOperationException(),
			};

			// Assert
			Assert.Equal(expected, actual);
		}
	}
}
