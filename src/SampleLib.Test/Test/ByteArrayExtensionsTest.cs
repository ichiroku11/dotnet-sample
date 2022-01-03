using Xunit;

namespace SampleLib.Test;

public class ByteArrayExtensionsTest {
	[Theory]
	[InlineData(new byte[] { 0x01, 0x02 }, "0102")]
	[InlineData(new byte[] { 0x1a, 0x2b }, "1a2b")]
	public void ToHexString_動きを確認する(byte[] original, string expected) {
		// Arrange
		// Act
		var actual = original.ToHexString();

		// Assert
		Assert.Equal(expected, actual);
	}
}
