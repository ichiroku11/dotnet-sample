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

		// Assert
		Assert.Equal(-1, result);
		Assert.Equal(1, param.Value);
	}

	// https://learn.microsoft.com/ja-jp/sql/relational-databases/system-stored-procedures/sp-sequence-get-range-transact-sql
	[Fact]
	public async Task ExecuteSqlRawAsync_sp_sequence_get_rangeを使ってシーケンスの値を取得する() {
		// Arrange
		const string sql = @"
execute sp_sequence_get_range
	@sequence_name = @name,
	@range_size = @size,
	@range_first_Value = @first output,
	@range_last_Value = @last output,
	@sequence_increment = @increment output";

		var first = new SqlParameter("first", SqlDbType.Variant) {
			Direction = ParameterDirection.Output
		};

		var last = new SqlParameter("last", SqlDbType.Variant) {
			Direction = ParameterDirection.Output
		};

		var increment = new SqlParameter("increment", SqlDbType.Variant) {
			Direction = ParameterDirection.Output
		};

		// Act
		var result = await _context.Database.ExecuteSqlRawAsync(sql,
			// シーケンスオブジェクト名
			new SqlParameter("name", "dbo.SQ_Sample"),
			// 取得する値の数
			new SqlParameter("size", 5L),
			first,
			last,
			increment);

		// Assert
		Assert.Equal(-1, result);
		// シーケンスの型がbigintなのでlongと比較する必要がある様子
		Assert.Equal(1L, first.Value);
		Assert.Equal(5L, last.Value);
		Assert.Equal(1L, increment.Value);
		// sizeを指定しているのでfirstとincrementがわかれば
		// 生成されたシーケンスの値を導き出せるはず
	}
}
