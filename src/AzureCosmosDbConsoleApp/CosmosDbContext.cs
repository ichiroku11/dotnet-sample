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
			.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name)
			.AddConsole()
			.AddDebug();
	});

	private readonly string _connectionString;

	public CosmosDbContext(IConfiguration config) {
		_connectionString = config.GetConnectionString(Constants.ConnectionStringName) ?? throw new InvalidOperationException();
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
		optionsBuilder
			.UseLoggerFactory(_loggerFactory)
			.UseCosmos(
				connectionString: _connectionString,
				databaseName: Constants.TestDatabase.Id);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		// Order
		modelBuilder.Entity<Order>(entityBuilder => {
			entityBuilder
				.ToContainer(Constants.OrderContainer.Id)
				.HasNoDiscriminator()
				// パーティションキーを指定しない場合、
				// エンティティを削除するとDbUpdateExceptionが発生していた
				// HTTPレスポンスはNotFound(404)だった
				.HasPartitionKey(order => order.Id);

			entityBuilder.Property(order => order.Id).ToJsonProperty("id");
			entityBuilder.Property(order => order.CustomerId).ToJsonProperty("customerId");
			entityBuilder.Property(order => order.OrderedAt).ToJsonProperty("orderedAt");

			// OrderDetail
			entityBuilder.OwnsMany(
				order => order.Details,
				navigationBuilder => {
					navigationBuilder.ToJsonProperty("details");

					navigationBuilder.Property(detail => detail.Menu).ToJsonProperty("menu");
					navigationBuilder.Property(detail => detail.Price).ToJsonProperty("price");
					navigationBuilder.Property(detail => detail.Quantity).ToJsonProperty("quantity");
				});
		});
	}

	public DbSet<Order> Orders => Set<Order>();
}
