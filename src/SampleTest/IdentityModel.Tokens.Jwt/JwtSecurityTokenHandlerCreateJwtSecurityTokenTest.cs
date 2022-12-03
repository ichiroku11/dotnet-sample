using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SampleTest.IdentityModel.Tokens.Jwt;

public class JwtSecurityTokenHandlerCreateJwtSecurityTokenTest {
	private readonly ITestOutputHelper _output;

	public JwtSecurityTokenHandlerCreateJwtSecurityTokenTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void CreateJwtSecurityToken_ペイロードが空の署名なしトークンを生成する() {
		// Arrange
		var handler = new JwtSecurityTokenHandler {
			SetDefaultTimesOnTokenCreation = false,
		};

		// Act
		var token = handler.CreateJwtSecurityToken();

		// Assert
		Assert.Equal(@"{""alg"":""none"",""typ"":""JWT""}.{}", token.ToString());
	}

	[Fact]
	public void CreateJwtSecurityToken_ペイロードがissとaudだけの署名なしトークンを生成する() {
		// Arrange
		var handler = new JwtSecurityTokenHandler {
			SetDefaultTimesOnTokenCreation = false,
		};

		// Act
		var token = handler.CreateJwtSecurityToken(issuer: "i", audience: "a");

		// Assert
		Assert.Equal(@"{""alg"":""none"",""typ"":""JWT""}.{""iss"":""i"",""aud"":""a""}", token.ToString());
	}

	[Fact]
	public void CreateJwtSecurityToken_数値の配列を含んだトークンを生成する() {
		// Arrange
		var handler = new JwtSecurityTokenHandler {
			SetDefaultTimesOnTokenCreation = false,
		};
		var claims = new[] {
			new Claim("values", "1", ClaimValueTypes.Integer),
			new Claim("values", "2", ClaimValueTypes.Integer),
		};
		var identity = new ClaimsIdentity(claims);

		// Act
		var token = handler.CreateJwtSecurityToken(subject: identity);

		// Assert
		// 想定した結果（"values"の値が数値の配列）になる
		Assert.Equal(@"{""alg"":""none"",""typ"":""JWT""}.{""values"":[1,2]}", token.ToString());
	}

	[Fact]
	public void CreateJwtSecurityToken_文字列の配列を含んだトークンを生成する() {
		// Arrange
		var handler = new JwtSecurityTokenHandler {
			SetDefaultTimesOnTokenCreation = false,
		};
		var claims = new[] {
			new Claim("values", "x"),
			new Claim("values", "y"),
		};
		var identity = new ClaimsIdentity(claims);

		// Act
		var token = handler.CreateJwtSecurityToken(subject: identity);

		// Assert
		// 想定した結果（"values"の値が文字列の配列）になる
		Assert.Equal(@"{""alg"":""none"",""typ"":""JWT""}.{""values"":[""x"",""y""]}", token.ToString());
	}

	// クレームにJSONオブジェクトを格納したいがJSON文字列ではダメだった
	[Fact]
	public void CreateJwtSecurityToken_クレームにオブジェクトのJSON文字列を含めても文字列のままになる() {
		// Arrange
		var handler = new JwtSecurityTokenHandler {
			SetDefaultTimesOnTokenCreation = false,
		};

		var json = JsonSerializer.Serialize(new { x = 1 });
		var claims = new[] {
			new Claim("obj", json),
		};
		var identity = new ClaimsIdentity(claims);

		// Act
		var token = handler.CreateJwtSecurityToken(subject: identity);

		// Assert
		// JSON文字列のまま格納されてしまう・・・
		Assert.Equal(@"{""alg"":""none"",""typ"":""JWT""}.{""obj"":""{\""x\"":1}""}", token.ToString());
	}

	[Fact]
	public void CreateJwtSecurityToken_キーが短いとHS256で署名するときに例外が発生する() {
		// Arrange
		// 文字列ベースでもう1文字いる様子
		var secret = Encoding.UTF8.GetBytes("0123456789abcde");
		var key = new SymmetricSecurityKey(secret);
		_output.WriteLine($"secret.Length: {secret.Length}");
		_output.WriteLine($"key.KeySize: {key.KeySize}");

		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var handler = new JwtSecurityTokenHandler {
			SetDefaultTimesOnTokenCreation = false,
		};

		// Act
		// Assert
		var exception = Assert.Throws<ArgumentOutOfRangeException>(() => {
			handler.CreateJwtSecurityToken(issuer: "i", audience: "a", signingCredentials: credentials);
		});
		_output.WriteLine(exception.Message);
	}
}
