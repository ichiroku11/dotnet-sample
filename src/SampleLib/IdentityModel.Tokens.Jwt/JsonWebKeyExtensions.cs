using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace SampleLib.IdentityModel.Tokens.Jwt;

public static class JsonWebKeyExtensions {
	// ECCurveを取得する
	// 参考
	// https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/dev/src/Microsoft.IdentityModel.Tokens/ECDsaAdapter.cs#L306
	public static ECCurve GetNamedECCurve(this JsonWebKey jwk) {
		return jwk.Crv switch {
			JsonWebKeyECTypes.P256 => ECCurve.NamedCurves.nistP256,
			JsonWebKeyECTypes.P384 => ECCurve.NamedCurves.nistP384,
			JsonWebKeyECTypes.P512 or
			JsonWebKeyECTypes.P521 => ECCurve.NamedCurves.nistP521,
			_ => throw new ArgumentException(),
		};
	}

	// ECParametersを取得する
	// 参考
	// https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/dev/src/Microsoft.IdentityModel.Tokens/ECDsaAdapter.cs#L264
	public static ECParameters GetECParameters(this JsonWebKey jwk) {
		// Ktyのチェック
		if (!JsonWebAlgorithmsKeyTypes.EllipticCurve.Equals(jwk.Kty)) {
			throw new ArgumentException();
		}

		// Crv, X, Yのチェック
		if (string.IsNullOrEmpty(jwk.Crv)) {
			throw new ArgumentException();
		}

		if (string.IsNullOrEmpty(jwk.X)) {
			throw new ArgumentException();
		}

		if (string.IsNullOrEmpty(jwk.Y)) {
			throw new ArgumentException();
		}

		return new ECParameters {
			Curve = jwk.GetNamedECCurve(),
			Q = {
				X = Base64UrlEncoder.DecodeBytes(jwk.X),
				Y = Base64UrlEncoder.DecodeBytes(jwk.Y)
			}
		};
	}
}
