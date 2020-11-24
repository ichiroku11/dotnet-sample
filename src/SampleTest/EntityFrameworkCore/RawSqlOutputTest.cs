using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.EntityFrameworkCore {
	[Collection("dbo.Sample")]
	public class RawSqlOutputTest : IDisposable {
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

		public RawSqlOutputTest() {
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
	(1, N'a');";
			_context.Database.ExecuteSqlRaw(sql);
		}

		private void DropTable() {
			var sql = @"drop table if exists dbo.Sample;";
			_context.Database.ExecuteSqlRaw(sql);
		}

		[Fact]
		public async Task FromSqlInterpolated_output句を使ったinsert文を実行して結果を取得できる() {
			FormattableString sql = @$"
insert into dbo.Sample(Id, Name)
output inserted.*
values ({2}, {"b"})";

			var samples = await _context.Samples
				.FromSqlInterpolated(sql)
				.ToListAsync();

			var sample = samples.Single();
			Assert.Equal(2, sample.Id);
			Assert.Equal("b", sample.Name);
		}

		[Fact]
		public async Task FromSqlInterpolated_output句を使ったupdate文を実行して結果を取得できる() {
			FormattableString sql = @$"
update dbo.Sample
set Id = {1}, Name = {"c"}
output inserted.*";

			var samples = await _context.Samples
				.FromSqlInterpolated(sql)
				.ToListAsync();

			var sample = samples.Single();
			Assert.Equal(1, sample.Id);
			Assert.Equal("c", sample.Name);
		}
	}
}
