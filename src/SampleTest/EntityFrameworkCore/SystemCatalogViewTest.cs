using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

	[Keyless]
	[Table("partitions", Schema = "sys")]
	public class SystemPartition {
		[Column("partition_id")]
		public long Id { get; set; }

		[Column("object_id")]
		public int ObjectId { get; set; }

		// テーブルの行数を取得するには
		// インデックスのIDからヒープとクラスター化インデックスを対象にするとよいみたい
		// https://learn.microsoft.com/ja-jp/sql/relational-databases/system-catalog-views/sys-partitions-transact-sql?view=sql-server-ver17
		// 0 = ヒープ
		// 1 = クラスター化インデックス
		// 2 以上 = 非クラスター化インデックス
		[Column("index_id")]
		public int IndexId { get; set; }

		[Column("partition_number")]
		public int PartitionNumber { get; set; }

		[Column("rows")]
		public long Rows { get; set; }
	}

	private class SampleDbContext(ITestOutputHelper output) : SqlServerDbContext {
		private readonly ITestOutputHelper _output = output;

		public DbSet<SystemSchema> Schemas => Set<SystemSchema>();

		public DbSet<SystemTable> Tables => Set<SystemTable>();

		public DbSet<SystemPartition> Partitions => Set<SystemPartition>();

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			base.OnConfiguring(optionsBuilder);

			optionsBuilder.LogTo(
				action: message => _output.WriteLine(message),
				categories: [DbLoggerCategory.Database.Command.Name],
				minimumLevel: LogLevel.Information);
		}

	}

	private readonly SampleDbContext _context;

	public SystemCatalogViewTest(ITestOutputHelper output) {
		_context = new(output);

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
		Assert.Contains(schemas, schema => schema.Name == "dbo");
	}

	[Fact]
	public async Task テーブルの情報を取得する() {
		// Arrange

		// Act
		var tables = await _context.Tables
			.Join(_context.Schemas,
				table => table.SchemaId,
				schema => schema.Id,
				(table, schema) => new { table.Name, Schema = new { schema.Name } })
			.ToListAsync();

		// Assert
		Assert.Contains(tables, table => table.Name == "Sample" && table.Schema.Name == "dbo");
	}

	// SQLでJOINだけを行う場合
	[Fact]
	public async Task テーブルの行数を取得する() {
		// Arrange

		// Act
		var items = await _context.Schemas
			.Join(_context.Tables,
				schema => schema.Id,
				table => table.SchemaId,
				(schema, table) => new { Schema = schema, Table = table })
			// ヒープとクラスター化インデックスが対象
			.Join(_context.Partitions.Where(partition => partition.IndexId == 0 || partition.IndexId == 1),
				schemaTable => schemaTable.Table.Id,
				partition => partition.ObjectId,
				(schemaTable, partition) => new { schemaTable.Schema, schemaTable.Table, Partition = partition })
			.ToListAsync();

		var actual = items
			.Where(item => item.Table.Name == "Sample" && item.Schema.Name == "dbo")
			.Sum(item => item.Partition.Rows);

		// Assert
		Assert.Equal(2, actual);
	}

	// SQLで集計までする場合
	[Fact]
	public async Task テーブルの行数を取得する別解() {
		// Arrange

		// Act
		var items = await _context.Schemas
			.Join(_context.Tables,
				schema => schema.Id,
				table => table.SchemaId,
				(schema, table) => new { Schema = schema, Table = table })
			// ヒープとクラスター化インデックスが対象
			.Join(_context.Partitions.Where(partition => partition.IndexId == 0 || partition.IndexId == 1),
				schemaTable => schemaTable.Table.Id,
				partition => partition.ObjectId,
				(schemaTable, partition) => new { schemaTable.Schema, schemaTable.Table, Partition = partition })
			.GroupBy(item => new { SchemaName = item.Schema.Name, TableName = item.Table.Name })
			.Select(grouping => new {
				grouping.Key.SchemaName,
				grouping.Key.TableName,
				Rows = grouping.Sum(item => item.Partition.Rows)
			})
			.ToListAsync();

		var actual = items
			.First(item => item.TableName == "Sample" && item.SchemaName == "dbo")
			.Rows;

		// Assert
		Assert.Equal(2, actual);
	}
}
