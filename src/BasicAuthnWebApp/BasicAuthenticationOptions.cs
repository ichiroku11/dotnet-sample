using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicAuthnWebApp {
	public class BasicAuthenticationOptions : AuthenticationSchemeOptions {
		public ICredentialsValidator CredentialsValidator { get; set; }
	}
}
