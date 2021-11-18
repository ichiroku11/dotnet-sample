using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SampleLib.AspNetCore;

public static class X509Certificate2Extensions {
	public static X509Certificate2 RemovePrivateKey(this X509Certificate2 certificate)
		=> new X509Certificate2(certificate.Export(X509ContentType.Cert));
}
