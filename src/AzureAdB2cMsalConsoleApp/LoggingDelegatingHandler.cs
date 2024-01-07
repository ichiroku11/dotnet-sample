using Microsoft.Extensions.Logging;

namespace AzureAdB2cMsalConsoleApp;

public class LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger) : DelegatingHandler {
	private readonly ILogger<LoggingDelegatingHandler> _logger = logger;

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
		_logger.LogInformation("{request}", request);
		if (request.Content != null) {
			_logger.LogInformation("{content}", await request.Content.ReadAsStringAsync());
		}

		var response = await base.SendAsync(request, cancellationToken);

		_logger.LogInformation("{response}", response);
		if (response.Content != null) {
			_logger.LogInformation("{content}", await response.Content.ReadAsStringAsync());
		}
		return response;
	}
}
