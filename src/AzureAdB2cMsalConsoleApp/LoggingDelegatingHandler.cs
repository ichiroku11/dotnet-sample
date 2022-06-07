using Microsoft.Extensions.Logging;

namespace AzureAdB2cMsalConsoleApp;

public class LoggingDelegatingHandler : DelegatingHandler {
	private readonly ILogger<LoggingDelegatingHandler> _logger;

	public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger) {
		_logger = logger;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
		_logger.LogInformation(request.ToString());
		if (request.Content != null) {
			_logger.LogInformation(await request.Content.ReadAsStringAsync());
		}

		var response = await base.SendAsync(request, cancellationToken);

		_logger.LogInformation(response.ToString());
		if (response.Content != null) {
			_logger.LogInformation(await response.Content.ReadAsStringAsync());
		}
		return response;
	}
}
