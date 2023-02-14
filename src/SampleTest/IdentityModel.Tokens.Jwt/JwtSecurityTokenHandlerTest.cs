using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace SampleTest.IdentityModel.Tokens.Jwt;

public class JwtSecurityTokenHandlerTest {
	private readonly ITestOutputHelper _output;

	public JwtSecurityTokenHandlerTest(ITestOutputHelper output) {
		_output = output;
	}


	[Fact]
	public void CanValidateToken_trueを返す() {
		// Arrange
		var handler = new JwtSecurityTokenHandler();

		// Act
		// Assert
		Assert.True(handler.CanValidateToken);
	}

	[Fact]
	public void CanWriteToken_trueを返す() {
		// Arrange
		var handler = new JwtSecurityTokenHandler();

		// Act
		// Assert
		Assert.True(handler.CanWriteToken);
	}

	[Theory]
	// null、空文字、空白はfalse
	[InlineData(null, false)]
	[InlineData("", false)]
	[InlineData(" ", false)]
	// header.payload.signatureの形式はtrue
	[InlineData("0.0.0", true)]
	[InlineData("a.a.a", true)]
	// 署名はオプション
	[InlineData("a.a.", true)]
	// ドットは2つ必要
	[InlineData("a.a", false)]
	public void CanReadToken_トークンの形式を判定する(string token, bool expected) {
		// Arrange
		var handler = new JwtSecurityTokenHandler();

		// Act
		// JSON compact serialization formatかどうかを判定する
		var actual = handler.CanReadToken(token);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void ReadJwtToken_トークンに含まれたオブジェクトの配列を取り出す() {
		// Arrange
		var handler = new JwtSecurityTokenHandler {
			SetDefaultTimesOnTokenCreation = false,
		};

		var descriptor = new SecurityTokenDescriptor {
			// クレームにオブジェクトの配列を追加する
			Claims = new Dictionary<string, object> {
				["test"] = new[] {
					new { x = 1 },
					new { x = 2 },
				},
			}
		};

		var tokenToWrite = handler.CreateJwtSecurityToken(descriptor);
		_output.WriteLine(tokenToWrite.ToString());

		var jwt = tokenToWrite.RawData;

		// Act
		var token = handler.ReadJwtToken(jwt);
		var claims = token.Claims.Where(claim => string.Equals(claim.Type, "test", StringComparison.Ordinal));

		// Assert
		Assert.Equal(2, claims.Count());
		AssertHelper.ContainsClaim(claims, "test", @"{""x"":1}", JsonClaimValueTypes.Json);
		AssertHelper.ContainsClaim(claims, "test", @"{""x"":2}", JsonClaimValueTypes.Json);
	}
}
