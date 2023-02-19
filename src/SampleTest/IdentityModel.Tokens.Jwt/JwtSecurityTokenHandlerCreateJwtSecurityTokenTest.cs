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
	public void CreateJwtSecurityToken_SecurityTokenDescriptorを使ってトークンに数値の配列を含める() {
		// Arrange
		var handler = new JwtSecurityTokenHandler {
			SetDefaultTimesOnTokenCreation = false,
		};

		var descriptor = new SecurityTokenDescriptor {
			// クレームに数値の配列を追加する
			Claims = new Dictionary<string, object> {
				["test"] = new[] { 1, 2 },
			}
		};

		// Act
		var token = handler.CreateJwtSecurityToken(descriptor);

		// Assert
		Assert.Equal(@"{""alg"":""none"",""typ"":""JWT""}.{""test"":[1,2]}", token.ToString());
	}

	[Fact]
	public void CreateJwtSecurityToken_SecurityTokenDescriptorを使ってトークンにオブジェクトを含める() {
		// Arrange
		var handler = new JwtSecurityTokenHandler {
			SetDefaultTimesOnTokenCreation = false,
		};

		var descriptor = new SecurityTokenDescriptor {
			// クレームにオブジェクトを追加する
			Claims = new Dictionary<string, object> {
				["test"] = new { x = 1 },
			}
		};

		// Act
		var token = handler.CreateJwtSecurityToken(descriptor);

		// Assert
		Assert.Equal(@"{""alg"":""none"",""typ"":""JWT""}.{""test"":{""x"":1}}", token.ToString());
	}

	[Fact]
	public void CreateJwtSecurityToken_SecurityTokenDescriptorを使ってトークンにオブジェクトの配列を含める() {
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

		// Act
		var token = handler.CreateJwtSecurityToken(descriptor);

		// Assert
		Assert.Equal(@"{""alg"":""none"",""typ"":""JWT""}.{""test"":[{""x"":1},{""x"":2}]}", token.ToString());
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

	[Fact]
	public void CreateJwtSecurityToken_SymmetricSecurityKeyで暗号化したトークンを生成する() {
		// Arrange
		var secret = Encoding.UTF8.GetBytes("0123456789abcdef0123456789abcdef");
		var descriptor = new SecurityTokenDescriptor {
			EncryptingCredentials = new EncryptingCredentials(
				new SymmetricSecurityKey(secret),
				// コンテンツを暗号化する鍵（CEK）の暗号化アルゴリズム
				JwtConstants.DirectKeyUseAlg,
				// コンテンツの暗号化アルゴリズム
				SecurityAlgorithms.Aes128CbcHmacSha256),
		};

		var handler = new JwtSecurityTokenHandler {
			// とりあえずnbf、exp、iatを含めない
			SetDefaultTimesOnTokenCreation = false,
		};

		// Act
		var token = handler.CreateJwtSecurityToken(descriptor);

		// Assert
		// トークン
		{
			_output.WriteLine(token.ToString());
			_output.WriteLine(token.RawData);
			// シリアラズした結果はJWE（暗号化された）フォーマットになる
			Assert.Matches(JwtConstants.JweCompactSerializationRegex, token.RawData);

			// ヘッダー
			Assert.Equal(4, token.Header.Count);
			Assert.Equal(JwtConstants.DirectKeyUseAlg, token.Header.Alg);
			Assert.Equal(SecurityAlgorithms.Aes128CbcHmacSha256, token.Header.Enc);
			Assert.Equal(JwtConstants.HeaderType, token.Header.Typ);
			Assert.Equal(JwtConstants.HeaderType, token.Header.Cty);

			// ペイロードは空
			Assert.Empty(token.Payload);

			// この暗号化の指定では、CEKは含まれていない
			Assert.Equal("", token.RawEncryptedKey);
		}

		// インナートークン
		{
			_output.WriteLine(token.InnerToken.ToString());
			_output.WriteLine(token.InnerToken.RawData);
			Assert.NotNull(token.InnerToken);

			// シリアラズした結果はJWSフォーマットになる
			Assert.Matches(JwtConstants.JsonCompactSerializationRegex, token.InnerToken.RawData);

			// ヘッダー
			Assert.Equal(2, token.InnerToken.Header.Count);
			Assert.Equal("none", token.InnerToken.Header.Alg);
			Assert.Equal(JwtConstants.HeaderType, token.InnerToken.Header.Typ);

			// ペイロードは空
			Assert.Empty(token.InnerToken.Payload);

			// 署名はしていないので空
			Assert.Equal("", token.InnerToken.RawSignature);
		}
	}
}
