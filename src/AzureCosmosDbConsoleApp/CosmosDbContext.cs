using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AzureCosmosDbConsoleApp;

// 参考
// https://docs.microsoft.com/ja-jp/ef/core/providers/cosmos/?tabs=dotnet-core-cli
// https://github.com/dotnet/EntityFramework.Docs/blob/main/samples/core/Cosmos/ModelBuilding/OrderContext.cs
public class CosmosDbContext : DbContext {
	private readonly string _connectionString;

	public CosmosDbContext(IConfiguration config) {
		_connectionString = config.GetConnectionString("Cosmos");
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseCosmos(
			connectionString: _connectionString,
			databaseName: "Test");

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		// todo:
		base.OnModelCreating(modelBuilder);
	}

	public DbSet<Order> Orders => Set<Order>();
}
