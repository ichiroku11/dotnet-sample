using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Security.Cryptography {
	public class HashAlgorithmNameTest {
		[Fact]
		public void SHA256_Nameプロパティの値を確認する() {
			// Arrange
			// Act
			// Assert
			Assert.Equal(nameof(HashAlgorithmName.SHA256), HashAlgorithmName.SHA256.Name);
		}
	}
}
