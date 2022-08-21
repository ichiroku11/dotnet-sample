namespace AzureCosmosDbConsoleApp;

public static class Constants {
	public const string ConnectionStringName = "Cosmos";

	public static class TestDatabase {
		public const string Id = "Test";
	}

	public static class OrderContainer {
		public const string Id = "Order";
		public const string PartitionKeyPath = "/id";
	}
}
