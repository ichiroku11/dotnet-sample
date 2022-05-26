using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAdB2cMsalConsoleApp;

public class LoggingDelegatingHandler : DelegatingHandler {
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
		return base.SendAsync(request, cancellationToken);
	}
}
