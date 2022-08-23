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

	public async Task RunAsync() {
		await Task.CompletedTask;

		// todo: 取得できない
		var orders = await _context.Orders.ToListAsync();
		foreach (var order in orders) {
			_logger.LogInformation(order.ToString());
		}
	}
}
