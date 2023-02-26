using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SampleTest.IdentityModel.Tokens.Jwt;

public class JwtHeaderTest {
	private readonly ITestOutputHelper _output;

	public JwtHeaderTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void Constructor_空のヘッダーを生成する() {
		// Arrange
		// Act
		var header = new JwtHeader();

		// Assert
		Assert.Empty(header);
		Assert.Null(header.Typ);
		Assert.Null(header.Alg);

		_output.WriteLine(header.SerializeToJson());
	}

	[Fact]
	public void Constructor_署名なしのヘッダーを生成する() {
		// Arrange
		// Act
		var header = new JwtHeader(signingCredentials: null);

		// Assert
		Assert.Equal(2, header.Count);
		Assert.Equal(JwtConstants.HeaderType, header.Typ);
		Assert.Equal("none", header.Alg);

		_output.WriteLine(header.SerializeToJson());
	}

	[Fact]
	public void Constructor_HS256署名付きのヘッダーを生成する() {
		// Arrange
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("x"));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		// Act
		var header = new JwtHeader(credentials);

		// Assert
		Assert.Equal(2, header.Count);
		Assert.Equal(JwtConstants.HeaderType, header.Typ);
		Assert.Equal(SecurityAlgorithms.HmacSha256, header.Alg);

		_output.WriteLine(header.SerializeToJson());
	}

	// 対称鍵とも共通鍵とも言うのかも
	[Fact]
	public void Constructor_対称鍵で暗号化されたヘッダーを生成する() {
		// Arrange
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("x"));

		// "alg"：CEKを暗号化するアルゴリズム
		// https://www.rfc-editor.org/rfc/rfc7516#section-4.1.1
		// "enc"：コンテンツを暗号化するアルゴリズム
		// https://www.rfc-editor.org/rfc/rfc7516#section-4.1.2
		var credentials = new EncryptingCredentials(key, "a", "e");

		// Act
		var header = new JwtHeader(credentials);

		// Assert
		Assert.Equal(4, header.Count);
		Assert.Equal(JwtConstants.HeaderType, header.Typ);
		Assert.Equal("a", header.Alg);
		Assert.Equal("e", header.Enc);
		// "cty"も"JWT"
		Assert.Equal(JwtConstants.HeaderType, header.Cty);

		_output.WriteLine(header.SerializeToJson());
	}

	[Fact]
	public void SerializeToJson_空のヘッダーをJSONにシリアライズする() {
		// Arrange
		var header = new JwtHeader();

		// Act
		var actual = header.SerializeToJson();

		// Assert
		Assert.Equal("{}", actual);
	}

	[Fact]
	public void Base64UrlEncode_空のヘッダーをBase64Urlにエンコードする() {
		// Arrange
		var header = new JwtHeader();

		// Act
		var actual = header.Base64UrlEncode();
		_output.WriteLine(actual);

		// Assert
		Assert.Equal(Base64UrlEncoder.Encode("{}"), actual);
	}
}
