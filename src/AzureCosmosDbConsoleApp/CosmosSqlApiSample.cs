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
public class CosmosSqlApiSample(IConfiguration config, ILogger<CosmosSqlApiSample> logger) {
	private readonly string _connectionString = config.GetConnectionString(Constants.ConnectionStringName)
		?? throw new InvalidOperationException();
	private readonly ILogger _logger = logger;

	// コンテナー一覧表示
	private async Task ListContainerAsync(Database database) {
		using var iterator = database.GetContainerQueryIterator<ContainerProperties>();

		var containerIds = new StringBuilder();
		while (iterator.HasMoreResults) {
			foreach (var container in await iterator.ReadNextAsync()) {
				containerIds.AppendLine(container.Id);
			}
		}

		_logger.LogInformation("{containerIds}", containerIds.ToString());
	}

	private async Task<Container> CreateContainerAsync(Database database)
	=> await database.CreateContainerAsync(
		id: Constants.OrderContainer.Id,
		partitionKeyPath: Constants.OrderContainer.PartitionKeyPath);

	private Task DeleteContainerAsync(Database database)
		=> database.GetContainer(Constants.OrderContainer.Id).DeleteContainerAsync();

	// アイテムの追加
	// https://docs.microsoft.com/ja-jp/azure/cosmos-db/sql/how-to-dotnet-create-item#create-an-item-asynchronously
	private async Task AddOrdersAsync(Container container, IEnumerable<Order> orders) {
		foreach (var orderToAdd in orders) {
			var response = await container.CreateItemAsync(orderToAdd);
			_logger.LogInformation("{requestCharge}", response.RequestCharge);
			_logger.LogInformation("{response}", response.ToJson());
		}
	}

	// OrderをIDで取得
	// https://docs.microsoft.com/ja-jp/azure/cosmos-db/sql/how-to-dotnet-read-item#read-an-item-asynchronously
	private async Task GetOrderByIdAsync(Container container, string id) {
		var response = await container.ReadItemAsync<Order>(id, new PartitionKey(id));
		_logger.LogInformation("{requestCharge}", response.RequestCharge);
		_logger.LogInformation("{response}", response.ToJson());
	}

	// 複数のOrderをIDで取得
	// https://docs.microsoft.com/ja-jp/azure/cosmos-db/sql/how-to-dotnet-read-item#read-multiple-items-asynchronously
	private async Task GetOrdersByIdsAsync(Container container, IEnumerable<string> ids) {
		var items = ids
			.Select(id => (id, new PartitionKey(id)))
			.ToList();
		var response = await container.ReadManyItemsAsync<Order>(items);
		_logger.LogInformation("{requestCharge}", response.RequestCharge);
		_logger.LogInformation("{response}", response.ToJson());
	}

	// Orderをクエリで取得
	// https://docs.microsoft.com/ja-jp/azure/cosmos-db/sql/how-to-dotnet-query-items#query-items-using-a-sql-query-asynchronously
	private async Task GetOrdersByCustomerIdAsync(Container container, string customerId) {
		var query = new QueryDefinition("select * from c where c.customerId = @customerId")
			.WithParameter("@customerId", customerId);
		using var iterator = container.GetItemQueryIterator<Order>(query);

		while (iterator.HasMoreResults) {
			var response = await iterator.ReadNextAsync();
			_logger.LogInformation("{requestCharge}", response.RequestCharge);
			_logger.LogInformation("{response}", response.ToJson());
		}
	}

	// OrderをLINQで取得
	// https://docs.microsoft.com/ja-jp/azure/cosmos-db/sql/how-to-dotnet-query-items#query-items-using-linq-asynchronously
	private async Task GetOrdersByCustomerIdUsingLinqAsync(Container container, string customerId) {
		using var iterator = container.GetItemLinqQueryable<Order>()
			.Where(order => order.CustomerId == customerId)
			.ToFeedIterator();

		while (iterator.HasMoreResults) {
			var response = await iterator.ReadNextAsync();
			_logger.LogInformation("{requestCharge}", response.RequestCharge);
			_logger.LogInformation("{response}", response.ToJson());
		}
	}

	// OrderDetail部分だけをクエリで取得
	private async Task GetOrderDetailPartsAsync(Container container) {
		// 結果はOrderDetail配列の配列になる
		var query = new QueryDefinition("select * from c.details");
		using var iterator = container.GetItemQueryIterator<IEnumerable<OrderDetail>>(query);

		while (iterator.HasMoreResults) {
			var response = await iterator.ReadNextAsync();
			_logger.LogInformation("{requestCharge}", response.RequestCharge);
			_logger.LogInformation("{response}", response.ToJson());
		}
	}

	// OrderDetail部分だけをinキーワードを使ったクエリで取得
	private async Task GetOrderDetailsAsync(Container container) {
		// 結果は平坦化されたOrderDetail配列になる
		var query = new QueryDefinition("select * from c in c.details");
		using var iterator = container.GetItemQueryIterator<OrderDetail>(query);

		while (iterator.HasMoreResults) {
			var response = await iterator.ReadNextAsync();
			_logger.LogInformation("{requestCharge}", response.RequestCharge);
			_logger.LogInformation("{response}", response.ToJson());
		}
	}

	// OrderDetail部分だけをLINQ（SelectMany）で取得
	private async Task GetOrderDetailsUsingLinqAsync(Container container) {
		using var iterator = container.GetItemLinqQueryable<Order>()
			.SelectMany(order => order.Details)
			.ToFeedIterator();
		while (iterator.HasMoreResults) {
			var response = await iterator.ReadNextAsync();
			_logger.LogInformation("{requestCharge}", response.RequestCharge);
			_logger.LogInformation("{response}", response.ToJson());
		}
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

		var orders = OrderProvider.GetOrders();

		// Orderの追加
		await AddOrdersAsync(container, orders);

		// OrderをIDで取得
		await GetOrderByIdAsync(container, orders.First().Id);
		await GetOrdersByIdsAsync(container, orders.Take(2).Select(order => order.Id));

		// Orderをクエリで取得
		await GetOrdersByCustomerIdAsync(container, "x");

		// OrderをLINQで取得
		await GetOrdersByCustomerIdUsingLinqAsync(container, "x");

		// OrderDetail部分だけをクエリで取得
		await GetOrderDetailPartsAsync(container);

		// OrderDetail部分だけをinキーワードを使ったクエリで取得
		await GetOrderDetailsAsync(container);

		// OrderDetail部分だけをLINQ（SelectMany）で取得
		await GetOrderDetailsUsingLinqAsync(container);
	}
}
