using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

// グローバルフィルターのサンプル
public class QueryFilterTest : IDisposable {
	private class Sample {
		public int Id { get; init; }
		public string? Name { get; init; }
	}

	private class SampleDbContext : SqlServerDbContext {
		public DbSet<Sample> Samples => Set<Sample>();

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Sample>()
				.ToTable(nameof(Sample))
				// グローバルフィルター
				.HasQueryFilter(entity => entity.Name != null);
		}
	}

	private readonly SampleDbContext _context = new();

	public void Dispose() {
		_context.Dispose();

		GC.SuppressFinalize(this);
	}

	[Fact]
	public async Task FromSqlRaw_HasQueryFilterを確認する() {
		var samples = await _context.Samples
			.FromSqlRaw("select 1 as Id, null as Name")
			.ToListAsync();
		var sample = samples.FirstOrDefault();

		Assert.Null(sample);
	}

	[Fact]
	public async Task FromSqlRaw_IgnoreQueryFiltersを確認する() {
		var samples = await _context.Samples
			.FromSqlRaw("select 1 as Id, null as Name")
			.IgnoreQueryFilters()
			.ToListAsync();
		var sample = samples.FirstOrDefault();

		Assert.NotNull(sample);
		Assert.Equal(1, sample?.Id);
		Assert.Null(sample?.Name);
	}
}
