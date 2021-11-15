using Microsoft.EntityFrameworkCore;
using SampleLib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.EntityFrameworkCore;

// 参考
// https://docs.microsoft.com/ja-jp/ef/core/saving/concurrency
// https://docs.microsoft.com/ja-jp/aspnet/core/data/ef-rp/concurrency
[Collection(CollectionNames.EfCoreSample)]
public class ConcurrencyConflictTest : IDisposable {
	private class Sample {
		public int Id { get; set; }

		public string Value { get; set; }

		[Timestamp]
		public byte[] Version { get; set; }
	}

	private class SampleDbContext : SqlServerDbContext {
		public DbSet<Sample> Samples { get; init; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Sample>().ToTable(nameof(Sample));
		}
	}

	private readonly ITestOutputHelper _output;
	private SampleDbContext _context;

	public ConcurrencyConflictTest(ITestOutputHelper output) {
		_output = output;
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
	Id int not null,
	Value nvarchar(10) not null,
	Version rowversion not null,
	constraint PK_Sample primary key(Id)
);

insert into dbo.Sample(Id, Value)
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
	public async Task SaveChangesAsync_更新が成功する動きを確認する() {
		// Arrange
		// Act
		// Assert
		var sample1 = await _context.Samples.FindAsync(1);
		_output.WriteLine(sample1.Version.ToHexString());
		Assert.Equal("a", sample1.Value);

		// 値を変更して更新する
		sample1.Value = "b";
		_context.Entry(sample1).State = EntityState.Modified;
		await _context.SaveChangesAsync();
		/*
		SET NOCOUNT ON;
		UPDATE [Sample] SET [Value] = @p0
		WHERE [Id] = @p1 AND [Version] = @p2;
		SELECT [Version]
		FROM [Sample]
		WHERE @@ROWCOUNT = 1 AND [Id] = @p1;
		*/
		_output.WriteLine(sample1.Version.ToHexString());

		// SQLを実行させるため
		_context.Entry(sample1).State = EntityState.Detached;

		// 値が更新されていることを確認する
		var sample2 = await _context.Samples.FindAsync(1);
		_output.WriteLine(sample1.Version.ToHexString());
		Assert.Equal("b", sample2.Value);
	}

	[Fact]
	public async Task SaveChangesAsync_DbUpdateConcurrencyExceptionがスローされることを確認する() {
		// Arrange
		// Act
		// Assert
		var sample1 = await _context.Samples.FindAsync(1);
		Assert.Equal("a", sample1.Value);
		var sample2 = new Sample {
			Id = sample1.Id,
			Value = sample1.Value,
			Version = sample1.Version.ToArray(),
		};
		_output.WriteLine(sample1.Version.ToHexString());
		_output.WriteLine(sample2.Version.ToHexString());

		// sample1.Valueを変更して更新する
		sample1.Value = "b";
		_context.Entry(sample1).State = EntityState.Modified;
		await _context.SaveChangesAsync();
		_output.WriteLine(sample1.Version.ToHexString());
		_output.WriteLine(sample2.Version.ToHexString());

		// sample1の追跡を解除する
		_context.Entry(sample1).State = EntityState.Detached;

		// sample2.Valueを変更して更新する
		sample2.Value = "c";
		_context.Entry(sample2).State = EntityState.Modified;
		var exception = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => {
			await _context.SaveChangesAsync();
		});
		_output.WriteLine(exception.Message);
	}
}
