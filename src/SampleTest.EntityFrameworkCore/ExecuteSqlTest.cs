using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace SampleTest.EntityFrameworkCore;

public class ExecuteSqlTest : IDisposable {
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

	public ExecuteSqlTest(ITestOutputHelper output) {
		_output = output;

		_context = new SampleDbContext(_output);
	}

	public void Dispose() {
		_context.Dispose();

		GC.SuppressFinalize(this);
	}

	[Fact]
	public async Task ExecuteSqlAsync_DirectionのOutputを使って値を取得する() {
		// Arrange
		// パラメーター名に"@"を付けなくてもよい様子
		var param = new SqlParameter("p", SqlDbType.Int) {
			Direction = ParameterDirection.Output,
		};
		FormattableString sql = $"set {param} = 1";

		// Act
	
		var result = await _context.Database.ExecuteSqlAsync(sql);
		// 実行されるSQL
		// set @p = 1

		// Assert
		Assert.Equal(-1, result);
		// SQLでsetした値を取得できる
		Assert.Equal(1, param.Value);
	}
}
