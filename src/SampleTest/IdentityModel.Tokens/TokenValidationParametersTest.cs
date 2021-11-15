using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.IdentityModel.Tokens;

public class TokenValidationParametersTest {
	[Fact]
	public void Constructor_デフォルト値を確認する() {
		// Arrange
		// Act
		var parameters = new TokenValidationParameters();

		// Assert
		Assert.False(parameters.ValidateActor);
		Assert.True(parameters.ValidateAudience);
		Assert.True(parameters.ValidateIssuer);
		Assert.False(parameters.ValidateIssuerSigningKey);
		Assert.True(parameters.ValidateLifetime);
		Assert.False(parameters.ValidateTokenReplay);
	}
}
