using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleTest.EntityFrameworkCore {
	public abstract class AppDbContext : DbContext {
		private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => {
			builder
				.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
				.AddConsole()
				.AddDebug();
		});

		protected AppDbContext() {
			// エンティティーの変更を追跡しない
			// 追跡したい場合はAsTrackingメソッドを使う
			ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseLoggerFactory(_loggerFactory);
		}
	}
}
