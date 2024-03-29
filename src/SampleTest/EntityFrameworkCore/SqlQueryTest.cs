using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

// 参考
// https://learn.microsoft.com/ja-jp/ef/core/querying/sql-queries#querying-scalar-non-entity-types
public class SqlQueryTest(ITestOutputHelper output) : IDisposable {
	private readonly ITestOutputHelper _output = output;
	private readonly SampleDbContext _context = new();

	private class SampleDbContext : SqlServerDbContext {
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
			// サブクエリになるため、出力カラム名を「Value」とする必要がある
			.SqlQuery<int>($"select 1 as Value")
			.FirstAsync();
		// 実行されるクエリ
		/*
		SELECT TOP(1) [t].[Value]
		FROM (
			select 1 as Value
		) AS [t]
		*/

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
