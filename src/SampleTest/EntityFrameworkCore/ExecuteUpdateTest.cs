using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace SampleTest.EntityFrameworkCore;

// https://learn.microsoft.com/ja-jp/ef/core/saving/execute-insert-update-delete
[Collection(CollectionNames.EfCoreSample)]
public class ExecuteUpdateTest : IDisposable {
	private class Sample {
		public int Id { get; init; }

		public string Name { get; init; } = "";

		[Timestamp]
		public byte[] Version { get; init; } = default!;
	}

	private class SampleDbContext : SqlServerDbContext {
		private readonly ITestOutputHelper _output;

		public SampleDbContext(ITestOutputHelper output) {
			_output = output;
		}

		public DbSet<Sample> Samples => Set<Sample>();

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			base.OnConfiguring(optionsBuilder);

			// この時点で_outputはnullなので注意
			optionsBuilder.LogTo(
				action: message => _output.WriteLine(message),
				categories: new[] {
					DbLoggerCategory.Database.Command.Name
				},
				minimumLevel: LogLevel.Information);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Sample>().ToTable(nameof(Sample));
		}
	}

	private readonly ITestOutputHelper _output;
	private readonly SampleDbContext _context;

	public ExecuteUpdateTest(ITestOutputHelper output) {
		_output = output;

		_context = new SampleDbContext(_output);

		DropTable();
		CreateTable();
	}

	public void Dispose() {
		DropTable();

		_context.Dispose();

		GC.SuppressFinalize(this);
	}

	private void CreateTable() {
		var sql = @"
create table dbo.[Sample](
	Id int not null,
	Name nvarchar(10) not null,
	Version rowversion not null,
	constraint PK_Sample primary key(Id)
);";

		_context.Database.ExecuteSqlRaw(sql);
	}

	private void DropTable() {
		var sql = "drop table if exists dbo.[Sample];";

		_context.Database.ExecuteSqlRaw(sql);
	}

	[Fact]
	public async Task ExecuteUpdateAsync_IDを指定して更新する() {
		// Arrange
		var sampleToAdd = new Sample { Id = 1, Name = "abc" };
		_context.Samples.Add(sampleToAdd);
		await _context.SaveChangesAsync();

		// Act
		var result = await _context.Samples
			// 条件にIDを指定する
			// Timestamp属性が設定されたカラムがあるからといって、条件に含まれることはない
			.Where(sample => sample.Id == 1)
			.ExecuteUpdateAsync(calls => calls.SetProperty(sample => sample.Name, "efg"));

		// 実行されるSQL
		/*
		UPDATE [s]
		SET [s].[Name] = N'efg'
		FROM [Sample] AS [s]
		WHERE [s].[Id] = 1
		*/

		var actual = await _context.Samples.FirstAsync(sample => sample.Id == 1);

		// Assert
		Assert.Equal(1, actual.Id);
		Assert.Equal("efg", actual.Name);
	}
}
