using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest;

public class BitConverterTest {
	[Fact]
	public void ToUInt32_IPアドレスのバイト配列ををuintに変換するとしたら() {
		// Arrange

		// Act
		// 127.0.0.1のイメージ
		var actual = BitConverter.ToUInt32(new byte[] { 1, 0, 0, 127 });

		// Assert
		var expected = (127u << 24) + 1u;
		Assert.Equal(expected, actual);
	}
}
