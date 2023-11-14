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
		// Act
		using var ecdsa = ECDsa.Create();

		// Assert
		Assert.Null(ecdsa.KeyExchangeAlgorithm);
		Assert.Equal(521, ecdsa.KeySize);
		Assert.Equal("ECDsa", ecdsa.SignatureAlgorithm);
	}

	public static TheoryData<ECCurve, int> GetTheoryData_Create_KeySize() {
		return new() {
			{ ECCurve.NamedCurves.nistP256, 256 },
			{ ECCurve.NamedCurves.nistP384, 384 },
			{ ECCurve.NamedCurves.nistP521, 521 }
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_Create_KeySize))]
	public void Create_ECCurveを指定して生成した場合のKeySizeを確認する(ECCurve curve, int expected) {
		// Arrange
		// Act
		using var ecdsa = ECDsa.Create(curve);

		// Assert
		Assert.Equal(expected, ecdsa.KeySize);
	}

	[Fact]
	public void Create_引数を省略して生成した場合のECCurveはnistP521になる(){
		// Arrange
		var expected = ECCurve.NamedCurves.nistP521;
		using var ecdsa = ECDsa.Create();

		// Act
		var actual = ecdsa.ExportParameters(false).Curve;
		_output.WriteLine(actual.Oid.Value);
		_output.WriteLine(actual.Oid.FriendlyName);

		// Assert
		Assert.Equal(ECCurve.ECCurveType.Named, actual.CurveType);
		Assert.Equal(expected.CurveType, actual.CurveType);
		Assert.Equal(expected.Oid.Value, actual.Oid.Value);
		Assert.Equal(expected.Oid.FriendlyName, actual.Oid.FriendlyName);
	}

	public static TheoryData<ECCurve> GetTheoryData_Create_Curve() {
		return new() {
			// Createの引数にCurveを渡すかどうか
			{ ECCurve.NamedCurves.nistP256 },
			{ ECCurve.NamedCurves.nistP384 },
			{ ECCurve.NamedCurves.nistP521 },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_Create_Curve))]
	public void Create_ECCurveを指定して生成した場合のECCurveを確認する(ECCurve expected) {
		// Arrange
		using var ecdsa = ECDsa.Create(expected);

		// Act
		var actual = ecdsa.ExportParameters(false).Curve;
		_output.WriteLine(actual.Oid.Value);
		_output.WriteLine(actual.Oid.FriendlyName);

		// Assert
		Assert.Equal(ECCurve.ECCurveType.Named, actual.CurveType);
		Assert.Equal(expected.CurveType, actual.CurveType);
		Assert.Equal(expected.Oid.Value, actual.Oid.Value);
		Assert.Equal(expected.Oid.FriendlyName, actual.Oid.FriendlyName);
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

	[Fact]
	public void VerifyData_署名を検証する_ExportParameters() {
		// Arrange
		// 署名用のECDsa
		using var signer = ECDsa.Create();

		// 検証用のECDsa
		using var verifier = ECDsa.Create(signer.ExportParameters(false));

		var data = Encoding.UTF8.GetBytes("あいうえお");
		var hashAlgorithm = HashAlgorithmName.SHA256;

		// 署名する
		var signature = signer.SignData(data, hashAlgorithm);

		// Act
		// 署名を検証する
		var actual = verifier.VerifyData(data, signature, hashAlgorithm);

		// Assert
		Assert.True(actual);
	}

	// パラメーターをSubjectPublicKeyInfo形式でエクスポート・インポートする
	[Fact]
	public void VerifyData_署名を検証する_ExportSubjectPublicKeyInfo() {
		// Arrange
		// 署名用のECDsa
		using var signer = ECDsa.Create();
		var publicKey = signer.ExportSubjectPublicKeyInfo();

		// 検証用のECDsa
		using var verifier = ECDsa.Create();
		verifier.ImportSubjectPublicKeyInfo(publicKey, out _);

		var data = Encoding.UTF8.GetBytes("あいうえお");
		var hashAlgorithm = HashAlgorithmName.SHA256;

		var signature = signer.SignData(data, hashAlgorithm);

		// Act
		var actual = verifier.VerifyData(data, signature, hashAlgorithm);

		// Assert
		Assert.True(actual);
	}

	[Fact]
	public void VerifyData_署名の検証に失敗することを確認にする() {
		// Arrange
		using var signer = ECDsa.Create();
		using var verifier = ECDsa.Create(signer.ExportParameters(false));

		var data = Encoding.UTF8.GetBytes("あいうえお");
		var hashAlgorithm = HashAlgorithmName.SHA256;

		var signature = signer.SignData(data, hashAlgorithm);

		// 最後の1バイトを書き換えてみる
		signature[^1]--;

		// Act
		var actual = verifier.VerifyData(data, signature, hashAlgorithm);

		// Assert
		Assert.False(actual);
	}
}
