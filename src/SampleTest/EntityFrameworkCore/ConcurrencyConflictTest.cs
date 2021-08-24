using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.EntityFrameworkCore {
	// 参考
	// https://docs.microsoft.com/ja-jp/ef/core/saving/concurrency
	// https://docs.microsoft.com/ja-jp/aspnet/core/data/ef-rp/concurrency
	[Collection(CollectionNames.EfCoreSample)]
	public class ConcurrencyConflictTest : IDisposable {
		private class Sample {
			public int Id { get; init; }
			public string Value { get; init; }

			// todo:
			public byte[] Version { get; init; }
		}

		private class SampleDbContext : AppDbContext {
			public DbSet<Sample> Samples { get; init; }

			protected override void OnModelCreating(ModelBuilder modelBuilder) {
				modelBuilder.Entity<Sample>().ToTable(nameof(Sample));
			}
		}

		private SampleDbContext _context;

		public ConcurrencyConflictTest() {
			_context = new SampleDbContext();

			DropTable();
			InitTable();
		}

		public void Dispose() {
			DropTable();

			if (_context != null) {
				_context.Dispose();
				_context = null;
			}
		}

		private void InitTable() {
			// todo:
			/*
			var sql = @"";
			_context.Database.ExecuteSqlRaw(sql);
			*/
		}

		private void DropTable() {
			// todo:
			/*
			var sql = @"";
			_context.Database.ExecuteSqlRaw(sql);
			*/
		}
	}
}
