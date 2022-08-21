using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AzureCosmosDbConsoleApp;

// 参考
// https://github.com/Azure/azure-cosmos-dotnet-v3/blob/master/Microsoft.Azure.Cosmos.Samples/Usage/ContainerManagement/Program.cs
// https://github.com/Azure/azure-cosmos-dotnet-v3/blob/master/Microsoft.Azure.Cosmos.Samples/Usage/ItemManagement/Program.cs
public class CosmosSqlApiSample {
	private readonly string _connectionString;
	private readonly ILogger _logger;

	public CosmosSqlApiSample(IConfiguration config, ILogger<CosmosSqlApiSample> logger) {
		_connectionString = config.GetConnectionString(Constants.ConnectionStringName);
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
		id: Constants.OrderContainer.Id,
		partitionKeyPath: Constants.OrderContainer.PartitionKeyPath);

	private Task DeleteContainerAsync(Database database)
		=> database.GetContainer(Constants.OrderContainer.Id).DeleteContainerAsync();

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
		var database = await client.CreateDatabaseIfNotExistsAsync(Constants.TestDatabase.Id);

		// DatabaseResponse型はDatabase型に暗黙的にキャストできる
		await ListContainerAsync(database);

		// コンテナーの削除と作成
		await DeleteContainerAsync(database);
		var container = await CreateContainerAsync(database);

		var orders = GetOrders();

		// アイテムの追加
		// https://docs.microsoft.com/ja-jp/azure/cosmos-db/sql/how-to-dotnet-create-item#create-an-item-asynchronously
		foreach (var orderToAdd in orders) {
			var response = await container.CreateItemAsync(orderToAdd);
			_logger.LogInformation(response.RequestCharge.ToString());
			_logger.LogInformation(response.ToJson());
		}

		/*
		// アイテムをIDで取得
		// https://docs.microsoft.com/ja-jp/azure/cosmos-db/sql/how-to-dotnet-read-item#read-an-item-asynchronously
		{
			var id = orders.First().Id;
			var response = await container.ReadItemAsync<Order>(id, new PartitionKey(id));
			_logger.LogInformation(response.RequestCharge.ToString());
			_logger.LogInformation(response.ToJson());
		}

		// 複数のアイテムをIDで取得
		// https://docs.microsoft.com/ja-jp/azure/cosmos-db/sql/how-to-dotnet-read-item#read-multiple-items-asynchronously
		{
			var items = orders.Take(2)
				.Select(order => (order.Id, new PartitionKey(order.Id)))
				.ToList();
			var response = await container.ReadManyItemsAsync<Order>(items);
			_logger.LogInformation(response.RequestCharge.ToString());
			_logger.LogInformation(response.ToJson());
		}
		*/

		// Orderをクエリで取得
		// https://docs.microsoft.com/ja-jp/azure/cosmos-db/sql/how-to-dotnet-query-items#query-items-using-a-sql-query-asynchronously
		{
			var query = new QueryDefinition("select * from c where c.customerId = @customerId")
				.WithParameter("@customerId", "x");
			using var iterator = container.GetItemQueryIterator<Order>(query);

			while (iterator.HasMoreResults) {
				var response = await iterator.ReadNextAsync();
				_logger.LogInformation(response.RequestCharge.ToString());
				_logger.LogInformation(response.ToJson());
			}
		}

		// OrderをLINQで取得
		// https://docs.microsoft.com/ja-jp/azure/cosmos-db/sql/how-to-dotnet-query-items#query-items-using-linq-asynchronously
		{
			using var iterator = container.GetItemLinqQueryable<Order>()
				.Where(order => order.CustomerId == "x")
				.ToFeedIterator();
			while (iterator.HasMoreResults) {
				var response = await iterator.ReadNextAsync();
				_logger.LogInformation(response.RequestCharge.ToString());
				_logger.LogInformation(response.ToJson());
			}
		}

		// OrderDetail部分だけをクエリで取得
		// 結果はOrderDetail配列の配列になる
		{
			var query = new QueryDefinition("select * from c.details");
			using var iterator = container.GetItemQueryIterator<IEnumerable<OrderDetail>>(query);

			while (iterator.HasMoreResults) {
				var response = await iterator.ReadNextAsync();
				_logger.LogInformation(response.RequestCharge.ToString());
				_logger.LogInformation(response.ToJson());
			}
		}

		// OrderDetail部分だけをinキーワードを使ったクエリで取得
		// 結果は平坦化されたOrderDetail配列になる
		{
			var query = new QueryDefinition("select * from c in c.details");
			using var iterator = container.GetItemQueryIterator<OrderDetail>(query);

			while (iterator.HasMoreResults) {
				var response = await iterator.ReadNextAsync();
				_logger.LogInformation(response.RequestCharge.ToString());
				_logger.LogInformation(response.ToJson());
			}
		}
	}
}
