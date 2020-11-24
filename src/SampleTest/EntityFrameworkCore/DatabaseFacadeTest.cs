using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.EntityFrameworkCore {
	public class DatabaseFacadeTest : IDisposable{
 		private AppDbContext _context;

		public DatabaseFacadeTest() {
			_context = new AppDbContext();
		}

		public void Dispose() {
			if (_context != null) {
				_context.Dispose();
				_context = null;
			}
		}

		[Fact]
		public async Task ExecuteSqlRawAsync_select文の結果を取得できない() {
			var result = await _context.Database.ExecuteSqlRawAsync("select 1");

			// 影響を受けた行はない
			Assert.Equal(-1, result);
		}

		[Fact]
		public async Task ExecuteSqlRawAsync_出力パラメータを使って結果を取得する() {
			// 出力パラメータ
			var param = new SqlParameter {
				Direction = ParameterDirection.Output,
				ParameterName = "result",
				SqlDbType = SqlDbType.Int,
			};
			var result = await _context.Database.ExecuteSqlRawAsync("set @result = (select 1)", param);

			// 影響を受けた行はない
			Assert.Equal(-1, result);
			// 出力パラメータから値を取得できる
			Assert.Equal(1, param.Value);
		}
	}
}
