using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

// SQL ServerのDBコンテキスト
public class SqlServerDbContext : AppDbContext {
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
		base.OnConfiguring(optionsBuilder);

		var connectionString = new SqlConnectionStringBuilder {
			DataSource = ".",
			InitialCatalog = "Test",
			IntegratedSecurity = true,
			// A connection was successfully established with the server, but then an error occurred during the login process
			// https://learn.microsoft.com/ja-jp/sql/connect/ado-net/sqlclient-troubleshooting-guide?view=sql-server-ver16#login-phase-errors
			TrustServerCertificate = true,
		}.ToString();
		optionsBuilder.UseSqlServer(connectionString);
	}
}
