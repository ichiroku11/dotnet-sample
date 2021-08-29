using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleTest.EntityFrameworkCore {
	// インメモリのDBコンテキスト
	public class InMemoryDbContext : AppDbContext {
		private readonly string _dbName;

		public InMemoryDbContext(string dbName) {
			_dbName = dbName;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			base.OnConfiguring(optionsBuilder);

			optionsBuilder.UseInMemoryDatabase(_dbName);
		}
	}
}
