using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest.Collections {
	public class ArrayTest {
		[Fact]
		public void Index_試してみる() {
			var numbers = "0123456789".ToCharArray();
			// 0番目
			Assert.Equal('0', numbers[0]);

			// numbers.Length-1番目
			Assert.Equal('9', numbers[^1]);
		}

		[Fact]
		public void Index_例外をスローする() {
			var numbers = "0123456789".ToCharArray();

			// ^0はnumbers.Lengthのことであり例外
			Assert.Throws<IndexOutOfRangeException>(() => {
				var never = numbers[^0];
			});
		}

		[Fact]
		public void Range_試してみる() {
			var numbers = "0123456789".ToCharArray();

			// すべて
			Assert.Equal("0123456789", numbers[..]);

			// 1番目から_numbers.Length-1番目
			Assert.Equal("12345678", numbers[1..^1]);

			// 最後の2つ
			Assert.Equal("89", numbers[^2..]);
		}
	}
}
