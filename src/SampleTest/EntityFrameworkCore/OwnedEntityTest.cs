using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleTest.EntityFrameworkCore {
	// 参考
	// https://docs.microsoft.com/ja-jp/ef/core/modeling/owned-entities
	public class OwnedEntityTest {
		// メールアドレス
		private record MailAddress(string Address, string Name);
		// メール
		private record Mail(int Id, MailAddress From, MailAddress To);

		private class MailDbContext : AppDbContext {
			public DbSet<Mail> Mail { get; init; }

			protected override void OnModelCreating(ModelBuilder modelBuilder) {
				// todo:
			}
		}

		private MailDbContext _context;

		public OwnedEntityTest() {
			_context = new MailDbContext();

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
			var sql = @"";
			// todo:
			_context.Database.ExecuteSqlRaw(sql);
		}

		private void DropTable() {
			var sql = @"";
			// todo:
			_context.Database.ExecuteSqlRaw(sql);
		}
	}
}
