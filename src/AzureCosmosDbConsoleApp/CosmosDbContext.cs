using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureCosmosDbConsoleApp;

// 参考
// https://docs.microsoft.com/ja-jp/ef/core/providers/cosmos/?tabs=dotnet-core-cli
// https://github.com/dotnet/EntityFramework.Docs/blob/main/samples/core/Cosmos/ModelBuilding/OrderContext.cs
public class CosmosDbContext : DbContext {
	private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => {
		builder
			.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
			.AddConsole()
			.AddDebug();
	});

	private readonly string _connectionString;

	public CosmosDbContext(IConfiguration config) {
		_connectionString = config.GetConnectionString(Constants.ConnectionStringName);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
		optionsBuilder.UseLoggerFactory(_loggerFactory);

		optionsBuilder.UseCosmos(
			connectionString: _connectionString,
			databaseName: Constants.TestDatabase.Id);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.HasDefaultContainer(Constants.OrderContainer.Id);

	}

	public DbSet<Order> Orders => Set<Order>();
}
