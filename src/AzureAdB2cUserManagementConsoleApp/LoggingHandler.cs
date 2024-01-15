using Microsoft.Extensions.Logging;

namespace AzureAdB2cUserManagementConsoleApp;

public class LoggingHandler(ILogger logger) : DelegatingHandler {
	private readonly ILogger _logger = logger;

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
		_logger.LogInformation("{request}", request);

		var response = await base.SendAsync(request, cancellationToken);

		_logger.LogInformation("{response}", response);
		return response;
	}
}
