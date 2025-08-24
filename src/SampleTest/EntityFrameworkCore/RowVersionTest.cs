using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace SampleTest.EntityFrameworkCore;

[Collection(CollectionNames.EfCoreSample)]
public class RowVersionTest : IDisposable {
	private class Sample {
		public int Id { get; init; }

		public string Name { get; init; } = "";

		// rowversion列をulongのプロパティにマッピングしてみる
		[Timestamp]
		public ulong Version { get; init; } = 0UL;
	}

	private class SampleDbContext(ITestOutputHelper output) : SqlServerDbContext {
		private readonly ITestOutputHelper _output = output;

		public DbSet<Sample> Samples => Set<Sample>();

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			base.OnConfiguring(optionsBuilder);

			// この時点で_outputはnullなので注意
			optionsBuilder.LogTo(
				action: message => _output.WriteLine(message),
				categories: [DbLoggerCategory.Database.Command.Name],
				minimumLevel: LogLevel.Information);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Sample>()
				.ToTable(nameof(Sample))
				.Property(entity => entity.Version).IsRowVersion();
		}
	}

	private readonly ITestOutputHelper _output;
	private readonly SampleDbContext _context;

	public RowVersionTest(ITestOutputHelper output) {
		_output = output;

		_context = new SampleDbContext(_output);

		DropTable();
		CreateTable();
	}

	public void Dispose() {
		DropTable();

		_context.Dispose();

		GC.SuppressFinalize(this);
	}

	private void CreateTable() {
		FormattableString sql = $@"
create table dbo.Sample(
	Id int not null,
	Name nvarchar(10) not null,
	Version rowversion not null,
	constraint PK_Sample primary key(Id)
);";

		_context.Database.ExecuteSql(sql);
	}

	private void DropTable() {
		FormattableString sql = $@"
drop table if exists dbo.Sample;";

		_context.Database.ExecuteSql(sql);
	}

	// https://learn.microsoft.com/ja-jp/ef/core/what-is-new/ef-core-8.0/whatsnew#numeric-rowversions-for-sql-azuresql-server
	// In EF8, it is easy to instead map rowversion columns to long or ulong properties.
	[Fact]
	public async Task Add_ExecuteUpdateAsync_rowversion列をulongのプロパティに割り当てできることを確認する() {
		// Arrange
		// Act
		// 追加
		_context.Samples.Add(new Sample { Id = 1, Name = "abc" });
		await _context.SaveChangesAsync();
		// Versionプロパティがlongだと例外が発生するが
		// System.InvalidCastException : Unable to cast object of type 'System.Byte[]' to type 'System.Int64'.
		// Versionプロパティがulongだと例外は発生しない

		var added = await _context.Samples.FirstAsync(sample => sample.Id == 1);
		_output.WriteLine($"added: {added.Version}");

		// 更新
		var rows = await _context.Samples
			.Where(sample => sample.Id == 1 && sample.Version == added.Version)
			.ExecuteUpdateAsync(calls => calls.SetProperty(sample => sample.Name, "efg"));

		var updated = await _context.Samples.FirstAsync(sample => sample.Id == 1);
		_output.WriteLine($"updated: {updated.Version}");

		// Assert
		Assert.Equal(1, rows);
		Assert.Equal("efg", updated.Name);
		Assert.NotEqual(added.Version, updated.Version);
	}

	[Fact]
	public async Task ExecuteUpdateAsync_rowversion列を使って楽観的同時実行制御の動きを確認する() {
		// Arrange
		_context.Samples.Add(new Sample { Id = 1, Name = "abc" });
		await _context.SaveChangesAsync();

		var first = await _context.Samples.FirstAsync(sample => sample.Id == 1);

		// Act
		var rows = await _context.Samples
			.Where(sample => sample.Id == 1 && sample.Version == first.Version + 1UL)
			.ExecuteUpdateAsync(calls => calls.SetProperty(sample => sample.Name, "efg"));

		var second = await _context.Samples.FirstAsync(sample => sample.Id == 1);

		// Assert
		Assert.Equal(0, rows);
		Assert.Equal(first.Name, second.Name);
		Assert.Equal(first.Version, second.Version);
	}
}
