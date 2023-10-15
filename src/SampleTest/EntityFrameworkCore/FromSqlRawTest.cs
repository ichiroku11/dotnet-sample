using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

// 参考
// https://docs.microsoft.com/ja-jp/ef/core/querying/raw-sql
[Collection(CollectionNames.EfCoreSample)]
public class FromSqlRawTest : IDisposable {
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

	public void Dispose() {
		_context.Dispose();

		GC.SuppressFinalize(this);
	}

	[Fact]
	public async Task FromSqlRaw_パラメータを使わないクエリ() {
		var samples = await _context.Samples
			.FromSqlRaw("select 1 as Id, N'a' as Name")
			.ToListAsync();
		var sample = samples.First();

		Assert.Equal(1, sample.Id);
		Assert.Equal("a", sample.Name);
	}

	[Fact]
	public async Task FromSqlRaw_パラメータを使ったクエリ() {
		var samples = await _context.Samples
			.FromSqlRaw(
				"select @id as Id, @name as Name",
				new SqlParameter("id", 1), new SqlParameter("name", "a"))
			.ToListAsync();
		var sample = samples.First();

		Assert.Equal(1, sample.Id);
		Assert.Equal("a", sample.Name);
	}
}
