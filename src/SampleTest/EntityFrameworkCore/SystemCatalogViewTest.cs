using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleTest.EntityFrameworkCore;

[Collection(CollectionNames.EfCoreSample)]
public class SystemCatalogViewTest : IDisposable {
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

	private readonly SampleDbContext _context = new();

	public SystemCatalogViewTest() {
		DropTable();
		InitTable();
	}

	public void Dispose() {
		DropTable();

		_context.Dispose();

		GC.SuppressFinalize(this);
	}

	private void InitTable() {
		FormattableString sql = $@"
create table dbo.Sample(
	Id int,
	Name nvarchar(10) not null,
	constraint PK_Sample primary key(Id)
);

insert into dbo.Sample(Id, Name)
output inserted.*
values
	(1, N'a'),
	(2, N'b');";

		_context.Database.ExecuteSql(sql);
	}

	private void DropTable() {
		FormattableString sql = $@"
drop table if exists dbo.Sample;";

		_context.Database.ExecuteSql(sql);
	}

	[Fact]
	public async Task スキーマの情報を取得する() {
		// Arrange

		// Act
		var schemas = await _context.Schemas.ToListAsync();

		// Assert
		Assert.Contains(schemas, s => s.Name == "dbo");
	}

	// todo: tables
	// todo: partitionsで行数
}
