using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleTest.EntityFrameworkCore {
	// SQL ServerのDBコンテキスト
	public class SqlServerDbContext : AppDbContext {
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			base.OnConfiguring(optionsBuilder);

			var connectionString = new SqlConnectionStringBuilder {
				DataSource = ".",
				InitialCatalog = "Test",
				IntegratedSecurity = true,
			}.ToString();
			optionsBuilder.UseSqlServer(connectionString);
		}
	}
}
