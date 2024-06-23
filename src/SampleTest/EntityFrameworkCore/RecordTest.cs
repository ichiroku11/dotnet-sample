using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

[Collection(CollectionNames.EfCoreSample)]
public class RecordTest : IDisposable {
	private record Sample(int Id, string Name);

	private class SampleDbContext : SqlServerDbContext {
		public DbSet<Sample> Samples => Set<Sample>();

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Sample>().ToTable(nameof(Sample));
		}
	}

	private readonly SampleDbContext _context = new();

	public RecordTest() {
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
	(1, N'a');";

		_context.Database.ExecuteSql(sql);
	}

	private void DropTable() {
		FormattableString sql = $@"
drop table if exists dbo.Sample;";

		_context.Database.ExecuteSql(sql);
	}

	[Fact]
	public async Task FirstOrDefault_record型のモデルを取得する() {
		// Arrange
		// Act
		var sample = (await _context.Samples.FirstOrDefaultAsync())!;

		// Assert
		Assert.Equal(1, sample.Id);
		Assert.Equal("a", sample.Name);
	}

	[Fact]
	public async Task Add_record型のモデルを追加する() {
		// Arrange
		// Act
		_context.Samples.Add(new Sample(2, "b"));
		await _context.SaveChangesAsync();

		var samples = await _context.Samples
			.OrderBy(sample => sample.Id)
			.ToListAsync();

		// Assert
		Assert.Equal(new[] { new Sample(1, "a"), new Sample(2, "b") }, samples);
	}
}
