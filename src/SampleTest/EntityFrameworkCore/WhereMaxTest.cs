using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

[Collection(CollectionNames.EfCoreSample)]
public class WhereMaxTest : IDisposable {
	private class Sample {
		public int Id { get; init; }
		public string Name { get; init; } = "";
		public int Value { get; init; }
	}

	private class SampleDbContext : SqlServerDbContext {
		public DbSet<Sample> Samples => Set<Sample>();
		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Sample>().ToTable(nameof(Sample))
				.HasKey(entity => entity.Id);
		}
	}

	private readonly SampleDbContext _context = new();

	public WhereMaxTest() {
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
	Id int not null,
	Name nvarchar(10) not null,
	Value int not null,
	constraint PK_Sample primary key(Id)
);

insert into dbo.Sample(Id, Name, Value)
output inserted.*
values
	(1, N'a', 1),
	(2, N'b', 3),
	(3, N'c', 2),
	(4, N'd', 3),
	(5, N'e', 1);";

		_context.Database.ExecuteSql(sql);
	}

	private void DropTable() {
		FormattableString sql = $@"
drop table if exists dbo.Sample;";

		_context.Database.ExecuteSql(sql);
	}
}
