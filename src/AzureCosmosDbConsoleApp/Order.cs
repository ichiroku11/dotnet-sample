namespace AzureCosmosDbConsoleApp;

public record Order(string Id, string CustomerId, DateTime OrderedAt, IEnumerable<OrderDetail> Details);
