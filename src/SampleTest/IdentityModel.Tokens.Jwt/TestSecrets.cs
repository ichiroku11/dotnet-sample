
using System.Text;

namespace SampleTest.IdentityModel.Tokens.Jwt;

// テスト用のシークレット
public static class TestSecrets {
	public static byte[] Default() => Encoding.UTF8.GetBytes("0123456789abcdef0123456789abcdef");
}
