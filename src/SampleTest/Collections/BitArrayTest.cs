using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Collections {
	public class BitArrayTest {
		[Fact]
		public void Length_bool配列から生成したBitArrayの長さを確認する() {
			// Arrange
			// Act
			var bits = new BitArray(new [] { true, false });

			// Assert
			Assert.Equal(2, bits.Length);
		}

		[Fact]
		public void Length_byte配列から生成したBitArrayの長さは8() {
			// Arrange
			// Act
			var bits = new BitArray(new byte[] { 1 });

			// Assert
			Assert.Equal(8, bits.Length);
		}
	}
}
