using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace SampleTest.EntityFrameworkCore;

[Collection(CollectionNames.EfCoreSample)]
public class RowVersionTest : IDisposable {
	private class Sample {
		public int Id { get; init; }

		public string Name { get; init; } = "";

		// rowversion列をlongのプロパティにマッピングしてみる
		[Timestamp]
		public long Version { get; init; } = 0;
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
create table dbo.[Sample](
	Id int not null,
	Name nvarchar(10) not null,
	Version rowversion not null,
	constraint PK_Sample primary key(Id)
);";

		_context.Database.ExecuteSql(sql);
	}

	private void DropTable() {
		FormattableString sql = $@"
drop table if exists dbo.[Sample];";

		_context.Database.ExecuteSql(sql);
	}

	// https://learn.microsoft.com/ja-jp/ef/core/what-is-new/ef-core-8.0/whatsnew#numeric-rowversions-for-sql-azuresql-server
	// In EF8, it is easy to instead map rowversion columns to long or ulong properties.
	// とあるけど例外が発生する
	/*
	[Fact]
	public async Task rowversion列をlongのプロパティに割り当てできることを確認する() {
		// Arrange
		// Act
		_context.Samples.Add(new Sample { Id = 1, Name = "abc" });
		await _context.SaveChangesAsync();
		// この時点で例外
		// System.InvalidCastException : Unable to cast object of type 'System.Byte[]' to type 'System.Int64'.

		var added = await _context.Samples.FirstAsync(sample => sample.Id == 1);
		_output.WriteLine($"added: {added.Version}");
		await _context.Samples
			.Where(sample => sample.Id == 1)
			.ExecuteUpdateAsync(calls => calls.SetProperty(sample => sample.Name, "efg"));

		var updated = await _context.Samples.FirstAsync(sample => sample.Id == 1);
		_output.WriteLine($"updated: {updated.Version}");

		// Assert
		Assert.Equal("efg", updated.Name);
		Assert.False(added.Version == updated.Version);
	}
	*/
}
