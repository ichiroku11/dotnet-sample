using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureAdB2cConsoleApp;

public class LoggingHttpProvider : IHttpProvider {
	private readonly IHttpProvider _provider;
	private readonly ILogger _logger;

	public LoggingHttpProvider(IHttpProvider provider, ILogger logger) {
		_provider = provider;
		_logger = logger;
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

	public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken) {
		_logger.LogInformation(request.ToString());

		var response = await _provider.SendAsync(request, completionOption, cancellationToken);

		_logger.LogInformation(response.ToString());
		return response;
	}
}
