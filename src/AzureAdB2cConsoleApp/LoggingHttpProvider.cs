using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureAdB2cConsoleApp {
	public class LoggingHttpProvider : IHttpProvider {
		private readonly IHttpProvider _provider;

		public LoggingHttpProvider(IHttpProvider provider) {
			_provider = provider;
		}

		public ISerializer Serializer => _provider.Serializer;

		public TimeSpan OverallTimeout {
			get => _provider.OverallTimeout;
			set => _provider.OverallTimeout = value;
		}

		public void Dispose() {
			_provider.Dispose();
		}

		public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) {
			return _provider.SendAsync(request);
		}

		public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken) {
			return _provider.SendAsync(request, completionOption, cancellationToken);
		}
	}
}
