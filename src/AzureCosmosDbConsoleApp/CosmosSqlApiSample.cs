using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
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
		while (iterator.HasMoreResults) {
			foreach (var container in await iterator.ReadNextAsync()) {
				containerIds.AppendLine(container.Id);
			}
		}

		_logger.LogInformation(containerIds.ToString());
	}

	private async Task<Container> CreateContainerAsync(Database database)
	=> await database.CreateContainerAsync(
		id: _containerId,
		partitionKeyPath: _partitionKeyPath);

	private Task DeleteContainerAsync(Database database)
		=> database.GetContainer(_containerId).DeleteContainerAsync();

	private static IEnumerable<Order> GetOrders() {
		return new[] {
			new Order(
				Guid.NewGuid().ToString(),
				"x",
				DateTime.Now,
				new List<OrderDetail> {
					new ("純けい", 360m, 3),
					new ("しろ", 330m, 2),
					new ("若皮", 360m, 3),
				}),
			new Order(
				Guid.NewGuid().ToString(),
				"y",
				DateTime.Now,
				new List<OrderDetail> {
					new ("純けい", 360m, 5),
					new ("しろ", 330m, 4),
					new ("若皮", 360m, 3),
				}),
			new Order(
				Guid.NewGuid().ToString(),
				"y",
				DateTime.Now,
				new List<OrderDetail> {
					new ("純けい", 360m, 3),
					new ("若皮", 360m, 3),
				}),
			new Order(
				Guid.NewGuid().ToString(),
				"x",
				DateTime.Now,
				new List<OrderDetail> {
					new("純けい", 360m, 1),
					new("しろ", 330m, 2),
				}),
		};
	}


	public async Task RunAsync() {
		using var client = new CosmosClientBuilder(_connectionString)
			.WithSerializerOptions(new CosmosSerializationOptions {
				PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
			})
			.Build();

		// databaseはDatabaseResponse型
		var database = await client.CreateDatabaseIfNotExistsAsync(_databaseId);

		// DatabaseResponse型はDatabase型に暗黙的にキャストできる
		await ListContainerAsync(database);

		// コンテナーの削除と作成
		await DeleteContainerAsync(database);
		var container = await CreateContainerAsync(database);

		var orders = GetOrders();

		// アイテムの追加
		foreach (var orderToAdd in orders) {
			var response = await container.CreateItemAsync(orderToAdd);
			//_logger.LogInformation(response.Diagnostics.ToString());
			_logger.LogInformation(((Order)response).Id);
		}

		// アイテムをIDで取得
		{
			var id = orders.First().Id;
			var response = await container.ReadItemAsync<Order>(id, new PartitionKey(id));
			//_logger.LogInformation(response.Diagnostics.ToString());
			_logger.LogInformation(((Order)response).Id);
		}

		// 複数のアイテムをIDで取得
		{
			var items = orders.Take(2)
				.Select(order => (order.Id, new PartitionKey(order.Id)))
				.ToList();
			var response = await container.ReadManyItemsAsync<Order>(items);
			//_logger.LogInformation(response.Diagnostics.ToString());
			foreach (var order in response) {
				_logger.LogInformation(order.Id);
			}
		}
	}
}
