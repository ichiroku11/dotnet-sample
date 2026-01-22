using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleTest.EntityFrameworkCore;

public class SystemCatalogViewTest {
	[Keyless]
	[Table("schemas", Schema = "sys")]
	private class SystemSchema {
		[Column("schema_id")]
		public int Id { get; set; }

		[Column("name")]
		public string Name { get; set; } = "";
	}

	[Keyless]
	[Table("tables", Schema = "sys")]
	private class SystemTable {
		[Column("object_id")]
		public int Id { get; set; }

		[Column("name")]
		public string Name { get; set; } = "";

		[Column("schema_id")]
		public int SchemaId { get; set; }
	}

	private class SampleDbContext : SqlServerDbContext {
		public DbSet<SystemSchema> Schemas => Set<SystemSchema>();

		public DbSet<SystemTable> Tables => Set<SystemTable>();
	}
}
