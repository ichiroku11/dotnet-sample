using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

// 参考
// https://docs.microsoft.com/ja-jp/ef/core/querying/raw-sql
[Collection(CollectionNames.EfCoreSample)]
public class FromSqlTest : IDisposable {
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
	public async Task FromSql_パラメータを使わないクエリ() {
		var samples = await _context.Samples
			// パラメータを使わないとあまり補間の意味が無いかも
			.FromSql($"select 1 as Id, N'a' as Name")
			// ネストされたSQL文にならないように
			// ここでSQLを実行する
			.ToListAsync();
		var sample = samples.First();

		Assert.Equal(1, sample.Id);
		Assert.Equal("a", sample.Name);
	}

	[Fact]
	public async Task FromSql_パラメータを使ったクエリその1() {
		var samples = await _context.Samples
			.FromSql($"select {1} as Id, {"a"} as Name")
			.ToListAsync();
		// 実行されるSQL
		// select @p0 as Id, @p1 as Name
		var sample = samples.First();

		Assert.Equal(1, sample.Id);
		Assert.Equal("a", sample.Name);
	}

	// その1と同じ
	[Fact]
	public async Task FromSql_パラメータを使ったクエリその2() {
		// 同じ
		//var param = (id: 1, name: "a");
		var param = new { id = 1, name = "a" };
		var samples = await _context.Samples
			.FromSql($"select {param.id} as Id, {param.name} as Name")
			.ToListAsync();
		// 実行されるSQL
		// select @p0 as Id, @p1 as Name
		var sample = samples.First();

		Assert.Equal(1, sample.Id);
		Assert.Equal("a", sample.Name);
	}
}
