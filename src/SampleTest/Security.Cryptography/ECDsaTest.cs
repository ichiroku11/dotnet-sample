using System.Security.Cryptography;
using System.Text;

namespace SampleTest.Security.Cryptography;

public class ECDsaTest {
	private readonly ITestOutputHelper _output;

	public ECDsaTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void Create_生成したインスタンスの各プロパティを確認する() {
		// Arrange
		using var ecdsa = ECDsa.Create();

		// Act
		// Assert
		Assert.Null(ecdsa.KeyExchangeAlgorithm);
		Assert.Equal("ECDsa", ecdsa.SignatureAlgorithm);
	}

	[Fact]
	public void SignData_秘密鍵がない場合は例外が発生する() {
		// Arrange
		using var ecdsa = ECDsa.Create(ECDsa.Create().ExportParameters(false));

		var data = Encoding.UTF8.GetBytes("あいうえお");
		var hashAlgorithm = HashAlgorithmName.SHA256;

		// Act
		// Assert
		var exception = Assert.ThrowsAny<CryptographicException>(() => {
			ecdsa.SignData(data, hashAlgorithm);
		});

		_output.WriteLine(exception.GetType().FullName);
		_output.WriteLine(exception.Message);
	}
}
