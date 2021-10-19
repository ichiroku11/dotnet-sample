using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SampleLib.AspNetCore {
	// https://docs.microsoft.com/ja-jp/dotnet/core/additional-tools/self-signed-certificates-guide#create-a-self-signed-certificate
	// dotnet dev-certs https --clean
	// dotnet dev-certs https --trust
	public static class X509Certificate2Helper {
		private static X509Certificate2 GetCertificate(string subjectName, string issuerName, string friendlyName) {
			using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

			store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

			return store.GetCertificate(subjectName, issuerName, friendlyName);
		}

		public static X509Certificate2 GetDevelopmentCertificate()
			=> GetCertificate(
				subjectName: "localhost",
				issuerName: "localhost",
				friendlyName: "ASP.NET Core HTTPS development certificate");
	}
}
