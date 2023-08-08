using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace AzureAdB2cUserManagementConsoleApp;

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
		_logger.LogInformation("{request}", request);

		var response = await _provider.SendAsync(request, completionOption, cancellationToken);

		_logger.LogInformation("{response}", response);
		return response;
	}
}
