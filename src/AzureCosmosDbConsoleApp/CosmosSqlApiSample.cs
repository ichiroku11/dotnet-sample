using Microsoft.Azure.Cosmos;
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

	public async Task RunAsync() {
		_logger.LogInformation(_connectionString);

		using var client = new CosmosClient(_connectionString);

		var database = (Database)await client.CreateDatabaseIfNotExistsAsync("Test");

		_logger.LogInformation(database.Id);
	}
}
