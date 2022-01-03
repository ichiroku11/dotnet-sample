using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AzureAppInsightsWebApp.Models;

public class AppDbContext : DbContext {
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
		var connectionString = new SqlConnectionStringBuilder {
			DataSource = ".",
			InitialCatalog = "Test",
			IntegratedSecurity = true,
		}.ToString();
		optionsBuilder.UseSqlServer(connectionString);
	}
}
