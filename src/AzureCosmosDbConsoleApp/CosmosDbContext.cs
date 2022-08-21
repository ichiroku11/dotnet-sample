using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AzureCosmosDbConsoleApp;

// 参考
// https://docs.microsoft.com/ja-jp/ef/core/providers/cosmos/?tabs=dotnet-core-cli
// https://github.com/dotnet/EntityFramework.Docs/blob/main/samples/core/Cosmos/ModelBuilding/OrderContext.cs
public class CosmosDbContext : DbContext {
	private readonly string _connectionString;

	public CosmosDbContext(IConfiguration config) {
		_connectionString = config.GetConnectionString(Constants.ConnectionStringName);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseCosmos(
			connectionString: _connectionString,
			databaseName: Constants.TestDatabase.Id);

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		// todo:
		modelBuilder.HasDefaultContainer(Constants.OrderContainer.Id);

	}

	public DbSet<Order> Orders => Set<Order>();
}
