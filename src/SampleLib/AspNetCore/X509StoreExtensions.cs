using System.Security.Cryptography.X509Certificates;

namespace SampleLib.AspNetCore;

public static class X509StoreExtensions {
	public static X509Certificate2? GetCertificate(
		this X509Store store, string subjectName, string issuerName, string friendlyName) {

		var certificates = store.Certificates
			.Find(X509FindType.FindBySubjectName, subjectName, true)
			.Find(X509FindType.FindByIssuerName, issuerName, true);

		foreach (var certificate in certificates) {
			if (string.Equals(certificate.FriendlyName, friendlyName, StringComparison.OrdinalIgnoreCase)) {
				return certificate;
			}
		}

		return null;
	}
}
