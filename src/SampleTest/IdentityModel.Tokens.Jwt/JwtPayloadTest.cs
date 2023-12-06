using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;

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
		Assert.Null(payload.NotBefore);
		Assert.Null(payload.Iss);
		// "iat"クレームがない場合はDateTimeの最小値
		Assert.Equal(DateTime.MinValue, payload.IssuedAt);
		Assert.Null(payload.Expiration);
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
		Assert.Null(payload.NotBefore);
		Assert.Equal("i", payload.Iss);
		Assert.Equal(DateTime.MinValue, payload.IssuedAt);
		Assert.Null(payload.Expiration);
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
		Assert.Null(payload.NotBefore);
		Assert.Equal("i", payload.Iss);
		Assert.Equal(DateTime.MinValue, payload.IssuedAt);
		Assert.Null(payload.Expiration);
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
		Assert.Equal(1, payload.NotBefore);
		Assert.Equal("i", payload.Iss);
		Assert.Equal(DateTime.MinValue, payload.IssuedAt);
		Assert.Equal(2, payload.Expiration);
		var audience = Assert.Single(payload.Aud);
		Assert.Equal("a", audience);

		_output.WriteLine(payload.SerializeToJson());
	}

	[Fact]
	public void Constructor_claimsCollectionでInt32の配列を指定してペイロードを生成する() {
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
		// 配列をもつクレームは、IdentityModel 6.xではJSON形式の文字列だったが、7.xではJSON形式ではない
		// 6.x
		//Assert.Equal("[1,2]", claim.Value);
		// 7.x
		Assert.Equal(Array.Empty<int>().ToString(), claim.Value);

		// JSONには配列が出力される
		Assert.Equal(@"{""test"":[1,2]}", payload.SerializeToJson());
	}

	[Fact]
	public void Constructor_claimsCollectionでInt32のJsonArrayをJsonElementとして指定してペイロードを生成する() {
		// Arrange
		// Act
		var payload = new JwtPayload(
			issuer: null,
			audience: null,
			claims: null,
			claimsCollection: new Dictionary<string, object> {
				["test"] = new JsonArray(JsonValue.Create(1), JsonValue.Create(2)).Deserialize<JsonElement>(),
			},
			notBefore: null,
			expires: null,
			issuedAt: null);
		var claims = payload.Claims;

		// Assert
		// 配列の要素数分のクレームが追加される
		var claim1 = AssertHelper.ContainsClaim(claims, "test", "1");
		Assert.Equal("http://www.w3.org/2001/XMLSchema#integer32", claim1.ValueType);
		var claim2 = AssertHelper.ContainsClaim(claims, "test", "2");
		Assert.Equal("http://www.w3.org/2001/XMLSchema#integer32", claim2.ValueType);

		// JSONには配列が出力される
		Assert.Equal(@"{""test"":[1,2]}", payload.SerializeToJson());
	}

	[Fact]
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
		// オブジェクトをもつクレームは、IdentityModel 6.xではJSON形式の文字列だったが、7.xではJSON形式ではない
		// 6.x
		//Assert.Equal(@"{""x"":1}", claim.Value);
		// 7.x
		Assert.Equal(new { x = 1 }.ToString(), claim.Value);

		// IdentityModel 6.xではJSONにシリアライズできたが、7.xでは例外が発生するようようになった
		// 6.x
		// JSONにはオブジェクトが出力される
		//Assert.Equal(@"{""test"":{""x"":1}}", payload.SerializeToJson());
		// 7.x
		var exception = Record.Exception(() => {
			payload.SerializeToJson();
		});
		Assert.IsType<ArgumentException>(exception);
		_output.WriteLine(exception.Message);
	}

	[Fact]
	public void Constructor_claimsCollectionでJsonObjectをJsonElementとして指定してペイロードを生成する() {
		// Arrange
		// Act
		var payload = new JwtPayload(
			issuer: null,
			audience: null,
			claims: null,
			claimsCollection: new Dictionary<string, object> {
				["test"] = new JsonObject { ["x"] = 1 }.Deserialize<JsonElement>(),
			},
			notBefore: null,
			expires: null,
			issuedAt: null);
		var claims = payload.Claims;
		var claim = claims.Single(claim => string.Equals(claim.Type, "test", StringComparison.Ordinal));

		// Assert
		Assert.Single(claims);
		Assert.Equal(@"{""x"":1}", claim.Value);
		Assert.Equal("JSON", claim.ValueType);
		Assert.Equal(@"{""test"":{""x"":1}}", payload.SerializeToJson());
	}

	[Fact]
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

		// ディクショナリをもつクレームは、IdentityModel 6.xではJSON形式の文字列だったが、7.xではJSON形式ではない
		// 6.x
		//var claim1 = AssertHelper.ContainsClaim(claims, "test", @"{""x"":1}");
		//var claim2 = AssertHelper.ContainsClaim(claims, "test", @"{""x"":2}");
		// 7.x
		var claim1 = AssertHelper.ContainsClaim(claims, "test", new { x = 1 }.ToString()!);
		var claim2 = AssertHelper.ContainsClaim(claims, "test", new { x = 2 }.ToString()!);

		// ValueTypeの値は、JsonClaimValueTypes.Jsonではなく、匿名型の型名のような文字列？
		_output.WriteLine(claim1.ValueType);
		_output.WriteLine(claim2.ValueType);

		// IdentityModel 6.xではオブジェクトの配列としてシリアライズできたが、7.xでは文字列の配列としてシリアライズされる
		// 6.x
		//Assert.Equal(@"{""test"":[{""x"":1},{""x"":2}]}", payload.SerializeToJson());
		// 7.x
		Assert.Equal(@"{""test"":[""{ x = 1 }"",""{ x = 2 }""]}", payload.SerializeToJson());
	}

	// todo: claimsCollection：JsonArray => JsonElement

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
