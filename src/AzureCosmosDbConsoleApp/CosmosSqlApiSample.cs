using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureCosmosDbConsoleApp;

public class CosmosSqlApiSample {

	private readonly string _connectionString;
	private readonly ILogger _logger;

	public CosmosSqlApiSample(IConfiguration config, ILogger<CosmosSqlApiSample> logger) {
		_connectionString = config.GetConnectionString("Cosmos");
		_logger = logger;
	}

	public Task RunAsync() {
		_logger.LogInformation(_connectionString);

		return Task.CompletedTask;
	}
}
