using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace SampleTest.EntityFrameworkCore;

[Collection(CollectionNames.EfCoreSampleSequence)]
public class ExecuteSqlRawSequenceTest : IDisposable {
	private class SampleDbContext(ITestOutputHelper output) : SqlServerDbContext {
		private readonly ITestOutputHelper _output = output;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			base.OnConfiguring(optionsBuilder);

			optionsBuilder.LogTo(
				action: message => _output.WriteLine(message),
				categories: [DbLoggerCategory.Database.Command.Name],
				minimumLevel: LogLevel.Information);
		}
	}

	private readonly ITestOutputHelper _output;
	private readonly SampleDbContext _context;

	public ExecuteSqlRawSequenceTest(ITestOutputHelper output) {
		_output = output;

		_context = new SampleDbContext(_output);

		DropSequence();
		InitSequence();
	}

	private void InitSequence() {
		_context.Database.ExecuteSql($"create sequence dbo.SQ_Sample as bigint start with 1");
	}

	private void DropSequence() {
		_context.Database.ExecuteSql($"drop sequence if exists dbo.SQ_Sample");

	}

	public void Dispose() {
		DropSequence();

		_context.Dispose();

		GC.SuppressFinalize(this);
	}

	[Fact]
	public async Task ExecuteSqlRawAsync_DirectionのOutputを使ってシーケンスの値を取得する() {
		// Arrange
		const string sql = "set @next = next value for dbo.SQ_Sample";
		var param = new SqlParameter("next", SqlDbType.Int) {
			Direction = ParameterDirection.Output,
		};

		// Act
		var result = await _context.Database.ExecuteSqlRawAsync(sql, param);
		// 実行されるSQL
		// set @next = next value for dbo.SQ_Sample

		// Assert
		Assert.Equal(-1, result);
		Assert.Equal(1, param.Value);
	}
}
