using Microsoft.IdentityModel.Tokens;
using SampleLib.AspNetCore;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SampleTest.IdentityModel.Tokens.Jwt;

public class JwtSecurityTokenTest {
	private readonly ITestOutputHelper _output;

	public JwtSecurityTokenTest(ITestOutputHelper output) {
		_output = output;
	}

	public class TestDataForSignatureAlgorithm : IEnumerable<object?[]>, IDisposable {
		private X509Certificate2? _certificate;

		public TestDataForSignatureAlgorithm() {
			_certificate = X509Certificate2Helper.GetDevelopmentCertificate();
		}

		public void Dispose() {
			_certificate?.Dispose();
			_certificate = null;
		}

		public IEnumerator<object?[]> GetEnumerator() {
			// 署名アルゴリズムがnull
			yield return new object?[] {
					new JwtSecurityToken(new JwtHeader(), new JwtPayload()),
					null,
				};

			// none
			yield return new object[] {
					new JwtSecurityToken(),
					"none",
				};
			yield return new object[] {
					new JwtSecurityToken(new JwtHeader(signingCredentials: null), new JwtPayload()),
					"none",
				};

			// HS256
			var credentials1 = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("x")), SecurityAlgorithms.HmacSha256);
			yield return new object[] {
					new JwtSecurityToken(new JwtHeader(credentials1), new JwtPayload()),
					"HS256",
				};

			// RS256
			var credentials2 = new X509SigningCredentials(_certificate);
			yield return new object[] {
					new JwtSecurityToken(new JwtHeader(credentials2), new JwtPayload()),
					"RS256",
				};
			var credentials3 = new SigningCredentials(new X509SecurityKey(_certificate), SecurityAlgorithms.RsaSha256);
			yield return new object[] {
					new JwtSecurityToken(new JwtHeader(credentials3), new JwtPayload()),
					"RS256",
				};
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	[Theory]
	[ClassData(typeof(TestDataForSignatureAlgorithm))]
	public void SignatureAlgorithm_署名アルゴリズムを確認する(JwtSecurityToken token, string expected) {
		// Arrange
		// Act
		var actual = token.SignatureAlgorithm;

		// Assert
		Assert.Equal(expected, actual);
	}

	public static TheoryData<JwtSecurityToken, string> GetTheoryDataForToString() {
		return new() {
			{
				new JwtSecurityToken(),
				@"{""alg"":""none"",""typ"":""JWT""}.{}"
			},
			{
				new JwtSecurityToken(new JwtHeader(), new JwtPayload()),
				@"{}.{}"
			},
			{
				new JwtSecurityToken(new JwtHeader(signingCredentials: null), new JwtPayload()),
				@"{""alg"":""none"",""typ"":""JWT""}.{}"
			},
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataForToString))]
	public void ToString_トークンの文字列表現を確認する(JwtSecurityToken token, string expected) {
		// Arrange
		// Act
		var actual = token.ToString();

		// Assert
		Assert.Equal(expected, actual);
	}
}
