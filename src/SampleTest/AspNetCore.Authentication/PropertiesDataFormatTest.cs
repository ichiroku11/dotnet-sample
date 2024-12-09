using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;

namespace SampleTest.AspNetCore.Authentication;

public class PropertiesDataFormatTest {
	private class NoopProtector : IDataProtector {
		public IDataProtector CreateProtector(string purpose) => new NoopProtector();

		public byte[] Protect(byte[] plaintext) {
			// コピーしたバイト配列を返す
			var protectedData = new byte[plaintext.Length];
			plaintext.CopyTo(protectedData, 0);
			return protectedData;
		}

		public byte[] Unprotect(byte[] protectedData) {
			// コピーしたバイト配列を返す
			var plantext = new byte[protectedData.Length];
			protectedData.CopyTo(plantext, 0);
			return plantext;
		}
	}

	[Fact]
	public void Protect_Unprotect_動きを確認する() {
		// Arrange
		var expected = new AuthenticationProperties {
			RedirectUri = "redirect-uri",
		};
		var format = new PropertiesDataFormat(new NoopProtector());

		// Act
		var protectedData = format.Protect(expected);
		var actual = format.Unprotect(protectedData);

		// Assert
		Assert.NotNull(actual);
		Assert.Equal(expected.RedirectUri, actual.RedirectUri);
	}
}
