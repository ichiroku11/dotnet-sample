using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.IdentityModel.Tokens;

public class SigningCredentialsTest {
	[Fact]
	public void Constructor_キーとアルゴリズムでインスタンスを生成する() {
		// Arrange
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("x"));
		var algorithm = SecurityAlgorithms.HmacSha256;

		// Act
		var credentials = new SigningCredentials(key, algorithm);

		// Assert
		Assert.Equal(key, credentials.Key);
		Assert.Equal(algorithm, credentials.Algorithm);
	}
}
