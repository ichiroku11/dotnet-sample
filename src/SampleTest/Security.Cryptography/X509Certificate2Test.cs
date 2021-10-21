using SampleLib.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Security.Cryptography {
	public class X509Certificate2Test {
		private readonly ITestOutputHelper _output;

		public X509Certificate2Test(ITestOutputHelper output) {
			_output = output;
		}

		[Fact]
		public void PrivateKey_値を変更しようとすると例外が発生する() {
			// Arrange
			var certificate = X509Certificate2Helper.GetDevelopmentCertificate();

			// Act
			// Assert
			Assert.True(certificate.HasPrivateKey);
			var exception = Assert.Throws<PlatformNotSupportedException>(() => {
				certificate.PrivateKey = null;
			});
			_output.WriteLine(exception.Message);
		}
	}
}
