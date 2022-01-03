using System.Security.Cryptography.X509Certificates;

namespace SampleLib.AspNetCore;

// https://docs.microsoft.com/ja-jp/dotnet/core/additional-tools/self-signed-certificates-guide#create-a-self-signed-certificate
public static class X509Certificate2Helper {
	// 証明書を取得する
	private static X509Certificate2? GetCertificate(string subjectName, string issuerName, string friendlyName) {
		using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

		store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

		return store.GetCertificate(subjectName, issuerName, friendlyName);
	}

	// ASP.NET Core 開発用の証明書を取得する
	// dotnet dev-certs https --clean
	// dotnet dev-certs https --trust
	public static X509Certificate2? GetDevelopmentCertificate()
		=> GetCertificate(
			subjectName: "localhost",
			issuerName: "localhost",
			friendlyName: "ASP.NET Core HTTPS development certificate");
}
