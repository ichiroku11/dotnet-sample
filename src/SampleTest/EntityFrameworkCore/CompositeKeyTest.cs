using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.EntityFrameworkCore {
	// 複合主キー、複合外部キーを試す
	// 参考
	// https://docs.microsoft.com/ja-jp/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-composite-key%2Csimple-key#foreign-key
	[Collection(CollectionNames.EfCoreSample)]
	public class CompositeKeyTest : IDisposable {
		private class Sample {
			public int Id1 { get; init; }
			public int Id2 { get; init; }

			public string Value { get; init; }

			public List<SampleDetail> Details { get; init; }
		}

		private class SampleDetail {
			public int SampleId1 { get; init; }

			public int SampleId2 { get; init; }

			public int DetailNo { get; init; }

			public string Value { get; init; }
		}

		private class SampleDbContext : AppDbContext {
			public DbSet<Sample> Samples { get; init; }

			protected override void OnModelCreating(ModelBuilder modelBuilder) {
				modelBuilder.Entity<Sample>().ToTable(nameof(Sample))
					// 複合主キー
					.HasKey(entity => new { entity.Id1, entity.Id2 });

				modelBuilder.Entity<SampleDetail>().ToTable(nameof(SampleDetail))
					// 複合主キー
					.HasKey(entity => new { entity.SampleId1, entity.SampleId2, entity.DetailNo });
			}
		}

		private SampleDbContext _context;

		public CompositeKeyTest() {
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
	Id1 int not null,
	Id2 int not null,
	Value nvarchar(10) not null,
	constraint PK_Sample primary key(Id1, Id2)
);
create table dbo.SampleDetail(
	SampleId1 int not null,
	SampleId2 int not null,
	DetailNo int not null,
	Value nvarchar(10) not null,
	constraint PK_SampleDetail primary key(SampleId1, SampleId2, DetailNo),
	constraint FK_SampleDetail_Sample foreign key(SampleId1, SampleId2) references dbo.Sample(Id1, Id2)
);

insert into dbo.Sample(Id1, Id2, Value)
output inserted.*
values
	(1, 2, N'a');
insert into dbo.SampleDetail(SampleId1, SampleId2, DetailNo, Value)
output inserted.*
values
	(1, 2, 1, N'a-1'),
	(1, 2, 2, N'a-2');";
			_context.Database.ExecuteSqlRaw(sql);
		}

		private void DropTable() {
			var sql = @"
drop table if exists dbo.SampleDetail;
drop table if exists dbo.Sample;";
			_context.Database.ExecuteSqlRaw(sql);
		}
			
		[Fact]
		public async Task FirstOrDefault_モデルを単体で取得する() {
			// Arrange
			// Act
			var sample = await _context.Samples.FirstOrDefaultAsync();

			// Assert
			Assert.Equal(1, sample.Id1);
			Assert.Equal(2, sample.Id2);
			Assert.Equal("a", sample.Value);
			Assert.Null(sample.Details);
		}

		[Fact]
		public async Task FirstOrDefault_モデルを関連データとともに取得する() {
			// Arrange
			// Act
			var sample = await _context.Samples
				.Include(sample => sample.Details)
				.FirstOrDefaultAsync();

			// Assert
			Assert.Equal(1, sample.Id1);
			Assert.Equal(2, sample.Id2);
			Assert.Equal("a", sample.Value);
			Assert.Equal(2, sample.Details.Count);
			Assert.Single(sample.Details, detail => detail.SampleId1 == 1 && detail.SampleId2 == 2 && detail.DetailNo == 1 && detail.Value == "a-1");
			Assert.Single(sample.Details, detail => detail.SampleId1 == 1 && detail.SampleId2 == 2 && detail.DetailNo == 2 && detail.Value == "a-2");
		}
	}
}
