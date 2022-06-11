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
		Assert.Equal("JWT", header.Typ);
		Assert.Equal("none", header.Alg);

		_output.WriteLine(header.SerializeToJson());
	}

	[Fact]
	public void Constructor_HS256署名付きのヘッダーを生成する() {
		// Arrange
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("x"));
		var algorithm = SecurityAlgorithms.HmacSha256;
		var credentials = new SigningCredentials(key, algorithm);

		// Act
		var header = new JwtHeader(credentials);

		// Assert
		Assert.Equal("JWT", header.Typ);
		Assert.Equal("HS256", header.Alg);

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
