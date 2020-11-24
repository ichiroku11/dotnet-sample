using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.EntityFrameworkCore {
	[Collection("dbo.Sample")]
	public class DbSetQueryTest : IDisposable {
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

		public DbSetQueryTest() {
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
			var sql = @"
create table dbo.Sample(
	Id int,
	Name nvarchar(10),
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
		public async Task FindAsync_主キーで検索できる() {
			var sample = await _context.Samples.FindAsync(1);

			Assert.Equal(1, sample.Id);
			Assert.Equal("a", sample.Name);
		}

		[Fact]
		public async Task FindAsync_主キーで検索して見つからない場合はnull() {
			var sample = await _context.Samples.FindAsync(0);

			Assert.Null(sample);
		}

		[Fact]
		public async Task FirstOrDefaultAsync_述語を使って検索できる() {
			var sample = await _context.Samples.FirstOrDefaultAsync(entity => entity.Id == 1);

			Assert.Equal(1, sample.Id);
			Assert.Equal("a", sample.Name);
		}

		// SQLに変換できないメソッド
		private static bool Predicate(Sample entity, int value) => entity.Id == value;

		[Fact]
		public async Task FirstOrDefaultAsync_述語に変換できないメソッド呼び出しを指定するとInvalidOperationException() {
			await Assert.ThrowsAsync<InvalidOperationException>(async () => {
				await _context.Samples.FirstOrDefaultAsync(entity => Predicate(entity, 1));
			});
		}

		// Expression
		private static Expression<Func<Sample, bool>> PredicateExpression(int value)
			=> entity => entity.Id == value;

		[Fact]
		public async Task FirstOrDefaultAsync_述語にExpressionを指定して検索できる() {
			var sample = await _context.Samples.FirstOrDefaultAsync(PredicateExpression(1));

			Assert.Equal(1, sample.Id);
			Assert.Equal("a", sample.Name);
		}
	}
}
