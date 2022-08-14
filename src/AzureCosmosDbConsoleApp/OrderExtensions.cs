using Microsoft.Azure.Cosmos;
using System.Text.Json;

namespace AzureCosmosDbConsoleApp;

public static class OrderExtensions {
	private static readonly JsonSerializerOptions _options
		= new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true,
		};

	public static string ToJson(this IEnumerable<Order> orders) => JsonSerializer.Serialize(orders, _options);

	public static string ToJson(this Order order) => JsonSerializer.Serialize(order, _options);

	public static string ToJson(this ItemResponse<Order> response) => ToJson((Order)response);

	public static string ToJson(this IEnumerable<OrderDetail> details) => JsonSerializer.Serialize(details, _options);

	public static string ToJson(this IEnumerable<IEnumerable<OrderDetail>> details) => JsonSerializer.Serialize(details, _options);
}
