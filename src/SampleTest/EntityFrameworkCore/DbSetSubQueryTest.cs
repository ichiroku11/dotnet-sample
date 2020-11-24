using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.EntityFrameworkCore {
	public class DbSetSubQueryTest : IDisposable {
		private class MenuItem {
			public int Id { get; set; }
			public string Name { get; set; }
			public string Category { get; set; }
			public decimal Price { get; set; }

			public override string ToString()
				=> $"{nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Category)} = {Category}, {nameof(Price)} = {Price}";
		}

		private class SampleDbContext : AppDbContext {
			public DbSet<MenuItem> MenuItems { get; set; }

			protected override void OnModelCreating(ModelBuilder modelBuilder) {
				modelBuilder.Entity<MenuItem>().ToTable(nameof(MenuItem));
			}
		}

		private readonly ITestOutputHelper _output;
		private SampleDbContext _context;

		public DbSetSubQueryTest(ITestOutputHelper output) {
			_output = output;
			_context = new SampleDbContext();

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
			var sql = @"
create table dbo.MenuItem(
	Id int not null,
	Name nvarchar(6) not null,
	Category nvarchar(3) not null,
	Price decimal(3) not null,
	constraint PK_MenuItem primary key(Id)
);

insert into dbo.MenuItem(Id, Name, Category, Price)
output inserted.*
values
	(1, N'純けい', N'串焼き', 500),
	(2, N'しろ', N'串焼き', 400),
	(3, N'若皮', N'串焼き', 300),
	(4, N'串カツ', N'揚げ物', 400),
	(5, N'ポテトフライ', N'揚げ物', 200),
	(6, N'レンコン揚げ', N'揚げ物', 300);";
			_context.Database.ExecuteSqlRaw(sql);
		}

		private void DropTable() {
			var sql = @"drop table if exists dbo.MenuItem;";
			_context.Database.ExecuteSqlRaw(sql);
		}

		[Fact]
		public async Task サブクエリで平均Price以上のMenuItemを取得() {
			var items = await _context.MenuItems
				// 平均Price以上
				.Where(item => item.Price >= _context.MenuItems.Average(item => item.Price))
				.ToListAsync();

			// 実行されるSQL
			/*
			SELECT [m].[Id], [m].[Category], [m].[Name], [m].[Price]
			FROM [MenuItem] AS [m]
			WHERE [m].[Price] >= (
				SELECT AVG([m0].[Price])
				FROM [MenuItem] AS [m0])
			*/

			Assert.All(items, item => _output.WriteLine(item.ToString()));

			Assert.Equal(3, items.Count());
			Assert.Contains(items, item => string.Equals(item.Name, "純けい"));
			Assert.Contains(items, item => string.Equals(item.Name, "しろ"));
			Assert.Contains(items, item => string.Equals(item.Name, "串カツ"));
		}

		[Fact]
		public async Task 相関サブクエリでカテゴリ別の平均Price以上のMenuItemを取得() {
			var items = await _context.MenuItems
				// カテゴリごとの平均Price以上
				.Where(item1 => item1.Price >=
					// カテゴリごとの平均Price
					_context.MenuItems
						// 相関
						.Where(item2 => string.Equals(item2.Category, item1.Category))
						//.Where(item2 => item2.Category == item1.Category)
						.Average(item => item.Price))
				.ToListAsync();

			// 実行されるSQL
			/*
			SELECT [m].[Id], [m].[Category], [m].[Name], [m].[Price]
			FROM [MenuItem] AS [m]
			WHERE [m].[Price] >= (
				SELECT AVG([m0].[Price])
				FROM [MenuItem] AS [m0]
				WHERE ([m0].[Category] = [m].[Category]) OR ([m0].[Category] IS NULL AND [m].[Category] IS NULL))
			*/

			Assert.All(items, item => _output.WriteLine(item.ToString()));

			Assert.Equal(4, items.Count());
			Assert.Contains(items, item => string.Equals(item.Name, "純けい"));
			Assert.Contains(items, item => string.Equals(item.Name, "しろ"));
			Assert.Contains(items, item => string.Equals(item.Name, "串カツ"));
			Assert.Contains(items, item => string.Equals(item.Name, "レンコン揚げ"));
		}
	}
}
