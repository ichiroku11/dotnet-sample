using Microsoft.Extensions.Configuration;

namespace AzureCosmosDbConsoleApp;

public class CosmosSqlApiSample {

	private readonly string _connectionString;

	public CosmosSqlApiSample(IConfiguration config) {
		_connectionString = config.GetConnectionString("Cosmos");
	}

	public Task RunAsync() {
		Console.WriteLine(_connectionString);

		return Task.CompletedTask;
	}
}
