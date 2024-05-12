using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

[Collection(CollectionNames.EfCoreSample)]
public class FromSqlOutputTest : IDisposable {
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

	public FromSqlOutputTest() {
		DropTable();
		InitTable();
	}

	public void Dispose() {
		DropTable();

		_context.Dispose();

		GC.SuppressFinalize(this);
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
	(1, N'a');";
		_context.Database.ExecuteSqlRaw(sql);
	}

	private void DropTable() {
		var sql = @"drop table if exists dbo.Sample;";
		_context.Database.ExecuteSqlRaw(sql);
	}

	[Fact]
	public async Task FromSql_output句を使ったinsert文を実行して結果を取得できる() {
		FormattableString sql = $@"
insert into dbo.Sample(Id, Name)
output inserted.*
values ({2}, {"b"})";

		var samples = await _context.Samples
			.FromSql(sql)
			.ToListAsync();

		var sample = samples.Single();
		Assert.Equal(2, sample.Id);
		Assert.Equal("b", sample.Name);
	}

	[Fact]
	public async Task FromSql_output句を使ったupdate文を実行して結果を取得できる() {
		FormattableString sql = $@"
update dbo.Sample
set Id = {1}, Name = {"c"}
output inserted.*";

		var samples = await _context.Samples
			.FromSql(sql)
			.ToListAsync();

		var sample = samples.Single();
		Assert.Equal(1, sample.Id);
		Assert.Equal("c", sample.Name);
	}
}
