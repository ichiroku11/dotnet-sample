namespace AzureCosmosDbConsoleApp;

public class Order {
	public string Id { get; init; } = "";

	public string CustomerId { get; init; } = "";

	public DateTime OrderedAt { get; init; }

	public List<OrderDetail> Details { get; init; } = new List<OrderDetail>();
}
