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
	public async Task ExecuteSqlRawAsync_DirectionのOutputを使って値を取得する() {
		// Arrange
		const string sql = "set @p = 1";
		var param = new SqlParameter("p", SqlDbType.Int) {
			Direction = ParameterDirection.Output,
		};

		// Act
		var result = await _context.Database.ExecuteSqlRawAsync(sql, param);
		// 実行されるSQL
		// set @p = 1

		// Assert
		// 戻り値は0ではない（ドキュメントに記載を見つけられず）
		Assert.Equal(-1, result);
		// SQLでsetした値を取得できる
		Assert.Equal(1, param.Value);
	}

	[Fact]
	public async Task ExecuteSqlRawAsync_DirectionのOutputを使う必要があるのに使わないとSqlExceptionが発生する() {
		// Arrange
		const string sql = "set @p = 1";
		// パラメーター名に"@"は不要みたい
		var param = new SqlParameter("p", SqlDbType.Int);

		// Act
		var exception = await Record.ExceptionAsync(async () => {
			await _context.Database.ExecuteSqlRawAsync(sql, param);
			// 実行されるSQL
			// set @p = 1
		});

		// Assert
		Assert.NotNull(exception);
		Assert.IsType<SqlException>(exception);
		_output.WriteLine(exception.Message);
		// パラメーター化クエリ '(@p int)set @p = 1' に必要なパラメーター '@p' が指定されていません。

		Assert.Null(param.Value);
	}

	// この使い方が良いのかはわからない
	[Fact]
	public async Task ExecuteSqlRawAsync_SqlParameterをSQL文に埋め込む() {
		// Arrange
		var param = new SqlParameter("@p", SqlDbType.Int) {
			Direction = ParameterDirection.Output,
		};

		// パラメーター名がSQL文に埋め込まれる
		var sql = $"set {param} = 1";

		// Act
		var result = await _context.Database.ExecuteSqlRawAsync(sql, param);
		// 実行されるSQL
		// set @p = 1

		// Assert
		Assert.Equal(-1, result);
		Assert.Equal(1, param.Value);
	}

	[Fact]
	public async Task ExecuteSqlRawAsync_SqlParameterをSQL文に埋め込むがSqlExceptionが発生する() {
		// Arrange
		// パラメーター名の接頭辞に"@"を付けないとSqlExceptionが発生する
		var param = new SqlParameter("p", SqlDbType.Int) {
			Direction = ParameterDirection.Output,
		};
		// パラメーター名がSQL文に埋め込まれるが
		// "@"で始まらない文字列なのでエラー
		var sql = $"set {param} = 1";

		// Act
		var exception = await Record.ExceptionAsync(async () => {
			await _context.Database.ExecuteSqlRawAsync(sql, param);
			// 実行されるSQL
			// set p = 1
		});

		// Assert
		Assert.NotNull(exception);
		Assert.IsType<SqlException>(exception);
		_output.WriteLine(exception.Message);
		// '=' 付近に不適切な構文があります。

		// nullではない？
		//Assert.Null(param.Value);
	}
}
