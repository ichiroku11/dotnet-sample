using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AzureCosmosDbConsoleApp;

public class CosmosEfCoreSample {
	private readonly CosmosDbContext _context;
	private readonly ILogger _logger;

	public CosmosEfCoreSample(CosmosDbContext context, ILogger<CosmosEfCoreSample> logger) {
		_context = context;
		_logger = logger;
	}

	// Orderを削除
	private async Task DeleteOrdersAsync() {
		var orders = await _context.Orders.ToListAsync();
		if (orders.Any()) {
			_logger.LogInformation(orders.ToJson());
			_context.Orders.RemoveRange(orders);
			await _context.SaveChangesAsync();
		}
	}

	// Orderを追加
	private async Task AddOrdersAsync(IEnumerable<Order> orders) {
		_logger.LogInformation(orders.ToJson());
		_context.AddRange(orders);
		await _context.SaveChangesAsync();
	}

	// OrderをIDで取得
	private async Task GetOrderByIdAsync(string id) {
		var order = await _context.Orders.FirstOrDefaultAsync(order => order.Id == id);
		_logger.LogInformation(order?.ToJson());
	}

	// 複数のOrderをIDで取得
	private async Task GetOrdersByIdsAsync(IEnumerable<string> ids) {
		var orders = await _context.Orders
			.Where(order => ids.Contains(order.Id))
			.ToListAsync();
		// 実行されるクエリ
		// SELECT c
		// FROM root c
		// WHERE c["id"] IN("{guid}", "{guid}")
		_logger.LogInformation(orders.ToJson());
	}

	// OrderをLINQで取得
	private async Task GetOrdersByCustomerIdAsync(string customerId) {
		var orders = await _context.Orders
			.Where(order => order.CustomerId == customerId)
			.ToListAsync();
		// 実行されるクエリ
		// SELECT c
		// FROM root c
		// WHERE(c["customerId"] = @__customerId_0)
		_logger.LogInformation(orders.ToJson());
	}

	// OrderをSQLクエリで取得
	private async Task GetOrdersByCustomerIdUsingFromSqlRawAsync(string customerId) {
		var orders = await _context.Orders
			// クエリはパラメタライズされる
			.FromSqlRaw("select * from c where c.customerId = {0}", customerId)
			.ToListAsync();
		// 実行されるクエリ
		// SELECT c
		// FROM (
		//     select * from c where c.customerId = @p0
		// ) c
		_logger.LogInformation(orders.ToJson());
	}

	public async Task RunAsync() {
		await _context.Database.EnsureCreatedAsync();

		// EF Coreではコンテナーの追加・削除はできない？

		// 既存のOrderをすべて削除
		await DeleteOrdersAsync();

		var orders = OrderProvider.GetOrders();

		// Orderを追加
		await AddOrdersAsync(orders);

		// OrderをIDで取得
		await GetOrderByIdAsync(orders.First().Id);
		await GetOrdersByIdsAsync(orders.Take(2).Select(order => order.Id));

		// OrderをLINQで取得
		await GetOrdersByCustomerIdAsync("x");

		// Orderをクエリで取得
		await GetOrdersByCustomerIdUsingFromSqlRawAsync("x");
	}
}
