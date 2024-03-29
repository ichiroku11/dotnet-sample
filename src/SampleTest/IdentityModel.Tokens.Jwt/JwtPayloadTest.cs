using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SampleTest.IdentityModel.Tokens.Jwt;

public class JwtPayloadTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

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
	public void Constructor_issuerとaudienceとnotBeforeとexpiresとissuedAtを指定してペイロードを生成する() {
		// Arrange
		// Act
		var payload = new JwtPayload(
			issuer: "i",
			audience: "a",
			claims: null,
			notBefore: EpochTime.UnixEpoch.AddSeconds(1),
			expires: EpochTime.UnixEpoch.AddSeconds(2),
			issuedAt: EpochTime.UnixEpoch.AddSeconds(3));

		// Assert
		Assert.Null(payload.Sub);
		Assert.Null(payload.Nonce);
		Assert.Equal(1, payload.NotBefore);
		Assert.Equal("i", payload.Iss);
		Assert.Equal(EpochTime.UnixEpoch.AddSeconds(3), payload.IssuedAt);
		Assert.Equal(2, payload.Expiration);
		var audience = Assert.Single(payload.Aud);
		Assert.Equal("a", audience);

		_output.WriteLine(payload.SerializeToJson());
	}

	[Fact]
	public void Constructor_claimsCollectionでInt32配列を指定してペイロードを生成する() {
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

	public static TheoryData<JsonElement> GetTheoryData_Constructor_Int32Array()
		=> new() {
			// JsonArrayからJsonElementを生成する
			new JsonArray(JsonValue.Create(1), JsonValue.Create(2)).Deserialize<JsonElement>(),
			// intの配列からJsonElementを生成する
			JsonSerializer.SerializeToElement(new[] { 1, 2 }),
		};

	[Theory]
	[MemberData(nameof(GetTheoryData_Constructor_Int32Array))]
	public void Constructor_claimsCollectionでInt32配列をJsonElementとして指定してペイロードを生成する(JsonElement element) {
		// Arrange
		// Act
		var payload = new JwtPayload(
			issuer: null,
			audience: null,
			claims: null,
			claimsCollection: new Dictionary<string, object> {
				["test"] = element,
			},
			notBefore: null,
			expires: null,
			issuedAt: null);
		var claims = payload.Claims;

		// Assert
		// 配列の要素数分のクレームが追加される
		AssertHelper.ContainsClaim(claims, "test", "1", ClaimValueTypes.Integer32);
		AssertHelper.ContainsClaim(claims, "test", "2", ClaimValueTypes.Integer32);

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

	public static TheoryData<JsonElement> GetTheoryData_Constructor_Object()
		=> new() {
			// JsonObjectからJsonElementを生成する
			new JsonObject { ["x"] = 1 }.Deserialize<JsonElement>(),
			// 匿名オブジェクトからJsonElementを生成する
			JsonSerializer.SerializeToElement(new { x = 1 }),
		};

	[Theory]
	[MemberData(nameof(GetTheoryData_Constructor_Object))]
	public void Constructor_claimsCollectionでオブジェクトをJsonElementとして指定してペイロードを生成する(JsonElement element) {
		// Arrange
		// Act
		var payload = new JwtPayload(
			issuer: null,
			audience: null,
			claims: null,
			claimsCollection: new Dictionary<string, object> {
				["test"] = element,
			},
			notBefore: null,
			expires: null,
			issuedAt: null);
		var claims = payload.Claims;
		var claim = claims.Single(claim => string.Equals(claim.Type, "test", StringComparison.Ordinal));

		// Assert
		Assert.Single(claims);

		// クレームにJSON文字列が格納され、ValueTypeは"JSON"になる
		Assert.Equal(@"{""x"":1}", claim.Value);
		Assert.Equal(JsonClaimValueTypes.Json, claim.ValueType);

		Assert.Equal(@"{""test"":{""x"":1}}", payload.SerializeToJson());
	}

	[Fact]
	public void Constructor_claimsCollectionでオブジェクト配列を指定してペイロードを生成する() {
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

	public static TheoryData<JsonElement> GetTheoryData_Constructor_ObjectArray()
	=> new() {
			new JsonArray {
				new JsonObject { ["x"] = 1 },
				new JsonObject { ["x"] = 2 },
			}.Deserialize<JsonElement>(),

			JsonSerializer.SerializeToElement(new[] {
				new { x = 1 },
				new { x = 2 }
			}),
	};

	[Theory]
	[MemberData(nameof(GetTheoryData_Constructor_ObjectArray))]
	public void Constructor_claimsCollectionでオブジェクト配列をJsonElementとして指定してペイロードを生成する(JsonElement element) {
		// Arrange
		// Act
		var payload = new JwtPayload(
			issuer: null,
			audience: null,
			claims: null,
			claimsCollection: new Dictionary<string, object> {
				["test"] = element
			},
			notBefore: null,
			expires: null,
			issuedAt: null);
		var claims = payload.Claims;

		// Assert
		Assert.Equal(2, claims.Count());

		// クレームにJSON文字列が格納され、ValueTypeは"JSON"になる
		AssertHelper.ContainsClaim(claims, "test", @"{""x"":1}", JsonClaimValueTypes.Json);
		AssertHelper.ContainsClaim(claims, "test", @"{""x"":2}", JsonClaimValueTypes.Json);

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
