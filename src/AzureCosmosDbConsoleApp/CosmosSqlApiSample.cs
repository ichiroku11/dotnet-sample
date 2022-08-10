using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AzureCosmosDbConsoleApp;

// 参考
// https://github.com/Azure/azure-cosmos-dotnet-v3/blob/master/Microsoft.Azure.Cosmos.Samples/Usage/ContainerManagement/Program.cs
public class CosmosSqlApiSample {
	private readonly string _connectionString;
	private readonly ILogger _logger;

	private const string _databaseId = "Test";
	private const string _containerId = "Order";
	private const string _partitionKeyPath = "/id";

	public CosmosSqlApiSample(IConfiguration config, ILogger<CosmosSqlApiSample> logger) {
		_connectionString = config.GetConnectionString("Cosmos");
		_logger = logger;
	}

	// コンテナー一覧表示
	private async Task ListContainerAsync(Database database) {
		using var iterator = database.GetContainerQueryIterator<ContainerProperties>();

		var containerIds = new StringBuilder();
		while(iterator.HasMoreResults) {
			foreach (var container in await iterator.ReadNextAsync()) {
				containerIds.AppendLine(container.Id);
			}
		}

		_logger.LogInformation(containerIds.ToString());
	}

	private Task CreateContainerAsync(Database database)
		=> database.CreateContainerAsync(
			id: _containerId,
			partitionKeyPath: _partitionKeyPath);

	private Task DeleteContainerAsync(Database database)
		=> database.GetContainer(_containerId).DeleteContainerAsync();

	public async Task RunAsync() {
		_logger.LogInformation(_connectionString);

		using var client = new CosmosClient(_connectionString);

		// databaseはDatabaseResponse型
		var database = await client.CreateDatabaseIfNotExistsAsync(_databaseId);

		// DatabaseResponse型はDatabase型に暗黙的にキャストできる
		await ListContainerAsync(database);

		// コンテナーの削除と作成
		await DeleteContainerAsync(database);
		await CreateContainerAsync(database);
	}
}
