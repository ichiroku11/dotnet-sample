using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.EntityFrameworkCore {
	[Collection("dbo.Sample")]
	public class RecordTest : IDisposable {
		private record Sample(int Id, string Name);

		private class SampleDbContext : AppDbContext {
			public DbSet<Sample> Samples { get; init; }

			protected override void OnModelCreating(ModelBuilder modelBuilder) {
				modelBuilder.Entity<Sample>().ToTable(nameof(Sample));
			}
		}

		private SampleDbContext _context;

		public RecordTest() {
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
		public async Task FirstOrDefault_record型のモデルを取得() {
			// Arrange
			// Act
			var sample = await _context.Samples.FirstOrDefaultAsync();

			// Assert
			Assert.Equal(1, sample.Id);
			Assert.Equal("a", sample.Name);
		}
	}
}
