using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

[Collection(CollectionNames.EfCoreSample)]
public class DbSetToQueryStringTest : IDisposable {
	private class Sample {
		public int Id { get; init; }
		public string Name { get; init; } = "";
	}

	private class SampleDbContext : SqlServerDbContext {
		public DbSet<Sample> Samples => Set<Sample>();

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Sample>().ToTable(nameof(Sample));
		}
	}

	private readonly SampleDbContext _context = new();
	private readonly ITestOutputHelper _output;

	public DbSetToQueryStringTest(ITestOutputHelper output) {
		_output = output;

		DropTable();
		InitTable();
	}

	public void Dispose() {
		DropTable();

		_context.Dispose();
	}

	private void InitTable() {
		var sql = @"
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
		_context.Database.ExecuteSqlRaw(sql);
	}

	private void DropTable() {
		var sql = @"drop table if exists dbo.Sample;";
		_context.Database.ExecuteSqlRaw(sql);
	}

	[Fact]
	public void ToQueryString_クエリの文字列を取得する() {
		// Arrange
		// Act
		var query = _context.Samples
			.FromSqlInterpolated($"select * from dbo.Sample")
			.ToQueryString();
		_output.WriteLine(query);

		// Assert
		// 改行などが含まれるようでEqualではない
		Assert.Contains("select * from dbo.Sample", query);
	}
}
