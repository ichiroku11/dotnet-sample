using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicAuthnWebApp {
	public class BasicAuthenticationPostConfigureOptions : IPostConfigureOptions<BasicAuthenticationOptions> {
		public void PostConfigure(string name, BasicAuthenticationOptions options) {
		}
	}
}
