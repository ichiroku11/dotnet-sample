using Microsoft.IdentityModel.Tokens;

namespace SampleTest.IdentityModel.Tokens;

public class Base64UrlEncoderTest {
	[Fact]
	public void Encode_変換を確認する() {
		// Arrange
		var bytes = new byte[] {
			0b_1111_1000
		};

		// Act
		// https://www.rfc-editor.org/rfc/rfc4648#section-5
		var actual = Base64UrlEncoder.Encode(bytes);

		// Assert
		Assert.Equal("-A", actual);
	}
}
