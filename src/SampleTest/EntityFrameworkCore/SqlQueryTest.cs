using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

// https://learn.microsoft.com/ja-jp/ef/core/querying/sql-queries#querying-scalar-non-entity-types
public class SqlQueryTest : IDisposable {
	private class SampleDbContext : SqlServerDbContext {
	}

	private readonly SampleDbContext _context = new();

	private readonly ITestOutputHelper _output;

	public SqlQueryTest(ITestOutputHelper output) {
		_output = output;
	}

	public void Dispose() {
		_context.Dispose();

		GC.SuppressFinalize(this);
	}

	[Fact]
	public async Task SqlQuery_スカラー値を返すクエリを実行する() {
		// Arrange
		// Act
		var actual = await _context.Database
			// 出力列名を「Value」とする必要がある様子
			.SqlQuery<int>($"select 1 as Value")
			.FirstAsync();

		// Assert
		Assert.Equal(1, actual);
	}

	[Fact]
	public async Task SqlQuery_スカラー値を返すクエリにValue句がないと例外が発生する() {
		// Arrange
		// Act
		var exception = await Record.ExceptionAsync(async () => {
			await _context.Database
				.SqlQuery<int>($"select 1")
				.FirstAsync();
		});

		// Assert
		Assert.IsType<SqlException>(exception);
		_output.WriteLine(exception.Message);
	}
}
