using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.EntityFrameworkCore {
	public class QueryFilterTest : IDisposable {
		private class Sample {
			public int Id { get; set; }
			public string Name { get; set; }
		}

		private class SampleDbContext : AppDbContext {
			public DbSet<Sample> Samples { get; set; }

			protected override void OnModelCreating(ModelBuilder modelBuilder) {
				modelBuilder.Entity<Sample>()
					.ToTable(nameof(Sample))
					// グローバルフィルタ
					.HasQueryFilter(entity => entity.Name != null);
			}
		}

		private SampleDbContext _context;

		public QueryFilterTest() {
			_context = new SampleDbContext();
		}

		public void Dispose() {
			if (_context != null) {
				_context.Dispose();
				_context = null;
			}
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

			Assert.Equal(1, sample.Id);
			Assert.Null(sample.Name);
		}
	}
}
