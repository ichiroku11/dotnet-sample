using Microsoft.IdentityModel.Tokens;
using SampleLib.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using Xunit;

namespace SampleLib.IdentityModel.Tokens.Jwt.Test;

public class JsonWebKeyExtensionsTest {
	// このチェックでいいのか
	private class ECParametersEqualityComparer : IEqualityComparer<ECParameters> {
		public bool Equals(ECParameters x, ECParameters y) {
			// Q
			if (!EnumerableHelper.BothNullOrSequenceEqual(x.Q.X, y.Q.X) ||
				!EnumerableHelper.BothNullOrSequenceEqual(x.Q.Y, y.Q.Y)) {
				return false;
			}

			// D
			if (!EnumerableHelper.BothNullOrSequenceEqual(x.D, y.D)) {
				return false;
			}

			// Curve
			if (!EnumerableHelper.BothNullOrSequenceEqual(x.Curve.A, y.Curve.A) ||
				!EnumerableHelper.BothNullOrSequenceEqual(x.Curve.B, y.Curve.B) ||
				!EnumerableHelper.BothNullOrSequenceEqual(x.Curve.G.X, y.Curve.G.X) ||
				!EnumerableHelper.BothNullOrSequenceEqual(x.Curve.G.Y, y.Curve.G.Y) ||
				!EnumerableHelper.BothNullOrSequenceEqual(x.Curve.Order, y.Curve.Order) ||
				!EnumerableHelper.BothNullOrSequenceEqual(x.Curve.Cofactor, y.Curve.Cofactor) ||
				!EnumerableHelper.BothNullOrSequenceEqual(x.Curve.Seed, y.Curve.Seed) ||
				x.Curve.CurveType != y.Curve.CurveType ||
				x.Curve.Hash != y.Curve.Hash ||
				!EnumerableHelper.BothNullOrSequenceEqual(x.Curve.Polynomial, y.Curve.Polynomial) ||
				!EnumerableHelper.BothNullOrSequenceEqual(x.Curve.Prime, y.Curve.Prime) ||
				x.Curve.Oid?.Value != y.Curve.Oid?.Value) {
				return false;
			}

			return true;
		}

		public int GetHashCode([DisallowNull] ECParameters obj) {
			// 使わないので未実装
			throw new NotImplementedException();
		}
	}

	[Fact]
	public void GetECParameters_正しく取得できることを確認する() {
		// Arrange
		using var ecdsa = ECDsa.Create();

		// 秘密鍵を含まないパラメーター
		var expected = ecdsa.ExportParameters(false);
		var jwk = JsonWebKeyConverter.ConvertFromECDsaSecurityKey(new ECDsaSecurityKey(ECDsa.Create(expected)));

		// Act
		var actual = jwk.GetECParameters();

		// Assert
		Assert.Null(jwk.D);
		// このチェックで正しいかは疑問
		Assert.Equal(expected, actual, new ECParametersEqualityComparer());
	}
}
