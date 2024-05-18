namespace AzureCosmosDbConsoleApp;

public static class OrderProvider {
	public static IEnumerable<Order> GetOrders() => [
		new Order {
			Id = Guid.NewGuid().ToString(),
			CustomerId = "x",
			OrderedAt = DateTime.Now,
			Details = [
				new("純けい", 360m, 3),
				new("しろ", 330m, 2),
				new("若皮", 360m, 3),
			]
		},
		new Order {
			Id = Guid.NewGuid().ToString(),
			CustomerId = "y",
			OrderedAt = DateTime.Now,
			Details = [
				new("純けい", 360m, 5),
				new("しろ", 330m, 4),
				new("若皮", 360m, 3),
			]
		},
		new Order {
			Id = Guid.NewGuid().ToString(),
			CustomerId = "y",
			OrderedAt = DateTime.Now,
			Details = [
				new("純けい", 360m, 3),
				new("若皮", 360m, 3),
			]
		},
		new Order {
			Id = Guid.NewGuid().ToString(),
			CustomerId = "x",
			OrderedAt = DateTime.Now,
			Details = [
				new("純けい", 360m, 1),
				new("しろ", 330m, 2),
			],
		}
	];
}
