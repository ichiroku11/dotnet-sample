using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SampleTest.IdentityModel.Tokens.Jwt;

public class JwtPayloadTest {
	private readonly ITestOutputHelper _output;

	public JwtPayloadTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void Constructor_空のペイロードを生成する() {
		// Arrange
		var payload = new JwtPayload {
		};

		// Act
		// Assert
		Assert.Null(payload.Sub);
		Assert.Null(payload.Nonce);
		Assert.Null(payload.Nbf);
		Assert.Null(payload.Iss);
		Assert.Null(payload.Iat);
		Assert.Null(payload.Exp);
		Assert.Empty(payload.Aud);

		_output.WriteLine(payload.SerializeToJson());
	}

	[Fact]
	public void Constructor_クレームからペイロードを生成する() {
		// Arrange
		var claims = new[] {
				new Claim(JwtRegisteredClaimNames.Iss, "i"),
				new Claim(JwtRegisteredClaimNames.Aud, "a"),
			};

		// Act
		var payload = new JwtPayload(claims);

		// Assert
		Assert.Null(payload.Sub);
		Assert.Null(payload.Nonce);
		Assert.Null(payload.Nbf);
		Assert.Equal("i", payload.Iss);
		Assert.Null(payload.Iat);
		Assert.Null(payload.Exp);
		var audience = Assert.Single(payload.Aud);
		Assert.Equal("a", audience);

		_output.WriteLine(payload.SerializeToJson());
	}

	[Fact]
	public void Constructor_issuerとaudienceからペイロードを生成する() {
		// Arrange
		// Act
		var payload = new JwtPayload(issuer: "i", audience: "a", claims: null, notBefore: null, expires: null);

		// Assert
		Assert.Null(payload.Sub);
		Assert.Null(payload.Nonce);
		Assert.Null(payload.Nbf);
		Assert.Equal("i", payload.Iss);
		Assert.Null(payload.Iat);
		Assert.Null(payload.Exp);
		var audience = Assert.Single(payload.Aud);
		Assert.Equal("a", audience);

		_output.WriteLine(payload.SerializeToJson());
	}

	[Fact]
	public void Constructor_issuerとaudienceとnotBeforeとexpiresを指定してペイロードを生成する() {
		// Arrange
		// Act
		var payload = new JwtPayload(
			issuer: "i",
			audience: "a",
			claims: null,
			notBefore: EpochTime.UnixEpoch.AddSeconds(1),
			expires: EpochTime.UnixEpoch.AddSeconds(2));

		// Assert
		Assert.Null(payload.Sub);
		Assert.Null(payload.Nonce);
		Assert.Equal(1, payload.Nbf);
		Assert.Equal("i", payload.Iss);
		Assert.Null(payload.Iat);
		Assert.Equal(2, payload.Exp);
		var audience = Assert.Single(payload.Aud);
		Assert.Equal("a", audience);

		_output.WriteLine(payload.SerializeToJson());
	}

	[Fact(Skip = "IdentityModel.7x")]
	public void Constructor_claimsCollectionで配列を指定してペイロードを生成する() {
		// Arrange
		// Act
		var payload = new JwtPayload(
			issuer: null,
			audience: null,
			claims: null,
			claimsCollection: new Dictionary<string, object> {
				["test"] = new[] { 1, 2 },
			},
			notBefore: null,
			expires: null,
			issuedAt: null);
		var claims = payload.Claims;
		var claim = claims.Single(claim => string.Equals(claim.Type, "test", StringComparison.Ordinal));

		// Assert
		Assert.Single(claims);
		Assert.Equal("[1,2]", claim.Value);
		// JSONには配列が出力される
		Assert.Equal(@"{""test"":[1,2]}", payload.SerializeToJson());
	}

	[Fact(Skip = "IdentityModel.7x")]
	public void Constructor_claimsCollectionでオブジェクトを指定してペイロードを生成する() {
		// Arrange
		// Act
		var payload = new JwtPayload(
			issuer: null,
			audience: null,
			claims: null,
			claimsCollection: new Dictionary<string, object> {
				["test"] = new { x = 1 },
			},
			notBefore: null,
			expires: null,
			issuedAt: null);
		var claims = payload.Claims;
		var claim = claims.Single(claim => string.Equals(claim.Type, "test", StringComparison.Ordinal));

		// Assert
		Assert.Single(claims);
		Assert.Equal(@"{""x"":1}", claim.Value);
		// JSONにはオブジェクトが出力される
		Assert.Equal(@"{""test"":{""x"":1}}", payload.SerializeToJson());
	}

	[Fact(Skip = "IdentityModel.7x")]
	public void Constructor_claimsCollectionでオブジェクトの配列を指定してペイロードを生成する() {
		// Arrange
		// Act
		var payload = new JwtPayload(
			issuer: null,
			audience: null,
			claims: null,
			claimsCollection: new Dictionary<string, object> {
				["test"] = new[] {
					new { x = 1 },
					new { x = 2 },
				}
			},
			notBefore: null,
			expires: null,
			issuedAt: null);
		var claims = payload.Claims;

		// Assert
		Assert.Equal(2, claims.Count());

		// ValueTypeの値は、JsonClaimValueTypes.Jsonではなく、匿名型の型名のような文字列？
		var claim = AssertHelper.ContainsClaim(claims, "test", @"{""x"":1}");
		_output.WriteLine(claim.ValueType);
		claim = AssertHelper.ContainsClaim(claims, "test", @"{""x"":2}");
		_output.WriteLine(claim.ValueType);

		// JSONにはオブジェクトが出力される
		Assert.Equal(@"{""test"":[{""x"":1},{""x"":2}]}", payload.SerializeToJson());
	}

	[Fact]
	public void SerializeToJson_空のペイロードをJSONにシリアライズする() {
		// Arrange
		var payload = new JwtPayload {
		};

		// Act
		var actual = payload.SerializeToJson();

		// Assert
		Assert.Equal("{}", actual);
	}

	[Fact]
	public void Base64UrlEncode_空のペイロードをBase64Urlにエンコードする() {
		// Arrange
		var payload = new JwtPayload {
		};

		// Act
		var actual = payload.Base64UrlEncode();
		_output.WriteLine(actual);

		// Assert
		Assert.Equal(Base64UrlEncoder.Encode("{}"), actual);
	}
}
