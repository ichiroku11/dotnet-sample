using Microsoft.Extensions.Logging;

namespace AzureAdB2cUserManagementConsoleApp;

public class LoggingHandler : DelegatingHandler {
	private readonly ILogger _logger;

	public LoggingHandler(ILogger logger) {
		_logger = logger;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
		_logger.LogInformation("{request}", request);

		var response = await base.SendAsync(request, cancellationToken);

		_logger.LogInformation("{response}", response);
		return response;
	}
}
