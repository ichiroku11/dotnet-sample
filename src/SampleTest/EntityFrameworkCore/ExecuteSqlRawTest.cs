using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace SampleTest.EntityFrameworkCore;

public class ExecuteSqlRawTest : IDisposable {
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

	public ExecuteSqlRawTest(ITestOutputHelper output) {
		_output = output;

		_context = new SampleDbContext(_output);
	}

	public void Dispose() {
		_context.Dispose();

		GC.SuppressFinalize(this);
	}

	[Fact]
	public async Task ExecuteSqlRawAsync_DirectionのOuputを使って値を取得する() {
		// Arrange
		const string sql = "set @p = 1";
		var param = new SqlParameter("p", SqlDbType.Int) {
			Direction = ParameterDirection.Output,
		};

		// Act
		var result = await _context.Database.ExecuteSqlRawAsync(sql, param);

		// Assert
		// 戻り値は0ではない（ドキュメントに記載を見つけられず）
		Assert.Equal(-1, result);
		// SQLでsetした値を取得できる
		Assert.Equal(1, param.Value);
	}
}
