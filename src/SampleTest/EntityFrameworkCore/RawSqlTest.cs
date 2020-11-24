using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.EntityFrameworkCore {
	// 参考
	// https://docs.microsoft.com/ja-jp/ef/core/querying/raw-sql
	public class RawSqlTest : IDisposable {
		private class Sample {
			public int Id { get; set; }
			public string Name { get; set; }
		}

		private class SampleDbContext : AppDbContext {
			public DbSet<Sample> Samples { get; set; }

			protected override void OnModelCreating(ModelBuilder modelBuilder) {
				modelBuilder.Entity<Sample>().ToTable(nameof(Sample));
			}
		}

		private SampleDbContext _context;

		public RawSqlTest() {
			_context = new SampleDbContext();
		}

		public void Dispose() {
			if (_context != null) {
				_context.Dispose();
				_context = null;
			}
		}

		// FromSqlInterpolated
		[Fact]
		public async Task FromSqlInterpolated_パラメータを使わないクエリ() {
			var samples = await _context.Samples
				// パラメータを使わないとあまり補間の意味が無いかも
				.FromSqlInterpolated($"select 1 as Id, N'a' as Name")
				// ネストされたSQL文にならないように
				// ここでSQLを実行する
				.ToListAsync();
			var sample = samples.First();

			Assert.Equal(1, sample.Id);
			Assert.Equal("a", sample.Name);
		}

		[Fact]
		public async Task FromSqlInterpolated_パラメータを使ったクエリその1() {
			var samples = await _context.Samples
				.FromSqlInterpolated($"select {1} as Id, {"a"} as Name")
				.ToListAsync();
			// 実行されるSQL
			// select @p0 as Id, @p1 as Name
			var sample = samples.First();

			Assert.Equal(1, sample.Id);
			Assert.Equal("a", sample.Name);
		}

		// その1と同じ
		[Fact]
		public async Task FromSqlInterpolated_パラメータを使ったクエリその2() {
			// 同じ
			//var param = (id: 1, name: "a");
			var param = new { id = 1, name = "a" };
			var samples = await _context.Samples
				.FromSqlInterpolated($"select {param.id} as Id, {param.name} as Name")
				.ToListAsync();
			// 実行されるSQL
			// select @p0 as Id, @p1 as Name
			var sample = samples.First();

			Assert.Equal(1, sample.Id);
			Assert.Equal("a", sample.Name);
		}

		// FromSqlRaw
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
}
