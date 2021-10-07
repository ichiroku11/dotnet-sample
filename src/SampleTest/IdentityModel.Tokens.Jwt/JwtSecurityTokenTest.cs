using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.IdentityModel.Tokens.Jwt {
	public class JwtSecurityTokenTest {
		private readonly ITestOutputHelper _output;

		public JwtSecurityTokenTest(ITestOutputHelper output) {
			_output = output;
		}

		public static IEnumerable<object[]> GetTestDataForToString() {
			yield return new object[] {
				new JwtSecurityToken(),
				@"{""alg"":""none"",""typ"":""JWT""}.{}",
			};
			yield return new object[] {
				new JwtSecurityToken(new JwtHeader(), new JwtPayload()),
				@"{}.{}",
			};
			yield return new object[] {
				new JwtSecurityToken(new JwtHeader(signingCredentials: null), new JwtPayload()),
				@"{""alg"":""none"",""typ"":""JWT""}.{}",
			};
		}

		[Theory]
		[MemberData(nameof(GetTestDataForToString))]
		public void ToString_トークンの文字列表現を確認する(JwtSecurityToken token, string expected) {
			// Arrange
			// Act
			var actual = token.ToString();

			// Assert
			Assert.Equal(expected, actual);
		}
	}
}
