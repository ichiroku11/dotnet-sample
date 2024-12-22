using System.Security.Cryptography.X509Certificates;

namespace SampleLib.AspNetCore;

public static class X509Certificate2Extensions {
	public static X509Certificate2 RemovePrivateKey(this X509Certificate2 certificate)
		=> X509CertificateLoader.LoadCertificate(certificate.Export(X509ContentType.Cert));
}
