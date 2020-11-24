using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.EntityFrameworkCore {
	// enumを文字列にマッピングするサンプル
	// 参考
	// https://docs.microsoft.com/ja-jp/ef/core/modeling/value-conversions
	[Collection("dbo.Monster")]
	public class EnumValueConversionTest {
		private enum MonsterCategory : byte {
			None = 0,
			Slime,
			Animal,
			Fly,
		}

		private static class MonsterCategoryHelper {
			// 最初から2文字の文字列
			public static string ToString(MonsterCategory value)
				=> value.ToString().Substring(0, 2).ToLower();

			// 2文字の文字列からenum値に（ちょっと雑）
			public static MonsterCategory ToEnum(string value)
				=> value switch
				{
					"sl" => MonsterCategory.Slime,
					"an" => MonsterCategory.Animal,
					"fl" => MonsterCategory.Fly,
					_ => MonsterCategory.None,
				};
		}

		private class Monster {
			public int Id { get; set; }
			public string Name { get; set; }
			public MonsterCategory Category { get; set; }
		}

		public enum ConvertType {
			// enum <=> tinyint
			Default = 0,
			// enum <=> varchar(10)
			String,
			// enum <=> char(2)
			Char2,
		}

		private class MonsterDbContext : AppDbContext {
			public DbSet<Monster> Monsters { get; set; }

			protected override void OnModelCreating(ModelBuilder modelBuilder) {
				modelBuilder.Entity<Monster>().ToTable(nameof(Monster));
			}
		}

		private class EnumStringMonsterDbContext : MonsterDbContext {
			protected override void OnModelCreating(ModelBuilder modelBuilder) {
				modelBuilder.Entity<Monster>().ToTable(nameof(Monster))
					.Property(entity => entity.Category)
					// enum <=> stringに変換する
					.HasConversion<string>();
			}
		}

		private class EnumChar2MonsterDbContext : MonsterDbContext {
			protected override void OnModelCreating(ModelBuilder modelBuilder) {
				modelBuilder.Entity<Monster>().ToTable(nameof(Monster))
					.Property(entity => entity.Category)
					.HasConversion(
						// enum <=> char[2]に変換する
						value => MonsterCategoryHelper.ToString(value),
						value => MonsterCategoryHelper.ToEnum(value));
			}
		}

		private static async Task CreateTableAsync(MonsterDbContext context) {
			var sql = context switch
			{
				EnumStringMonsterDbContext _ => @"
create table dbo.Monster(
	Id int,
	Name nvarchar(20),
	Category varchar(10),
	constraint PK_Monster primary key(Id)
);",
				EnumChar2MonsterDbContext _ => @"
create table dbo.Monster(
	Id int,
	Name nvarchar(20),
	Category char(2),
	constraint PK_Monster primary key(Id)
);",
				_ => @"
create table dbo.Monster(
	Id int,
	Name nvarchar(20),
	Category tinyint,
	constraint PK_Monster primary key(Id)
);",
			};
			await context.Database.ExecuteSqlRawAsync(sql);
		}

		private static async Task DropTableAsync(MonsterDbContext context) {
			var sql = @"drop table if exists dbo.Monster;";
			await context.Database.ExecuteSqlRawAsync(sql);
		}


		private static async Task AddAsync(MonsterDbContext context, params Monster[] monsters) {
			await context.Monsters.AddRangeAsync(monsters);
			await context.SaveChangesAsync();
		}

		private static async Task<Monster> FindAsync(MonsterDbContext context, int id) {
			return await context.Monsters.FirstOrDefaultAsync(monster => monster.Id == id);
		}

		[Theory]
		[InlineData(ConvertType.Default)]
		[InlineData(ConvertType.String)]
		[InlineData(ConvertType.Char2)]
		public async Task Enumは数値や文字列に変換できるAsync(ConvertType convertType) {
			using var context = convertType switch
			{
				ConvertType.String => new EnumStringMonsterDbContext(),
				ConvertType.Char2 => new EnumChar2MonsterDbContext(),
				_ => new MonsterDbContext(),
			};

			try {
				await DropTableAsync(context);
				await CreateTableAsync(context);

				var expected1 = new Monster {
					Id = 1,
					Name = "スライム",
					Category = MonsterCategory.Slime,
				};
				var expected2 = new Monster {
					Id = 2,
					Name = "ドラキー",
					Category = MonsterCategory.Fly,
				};
				await AddAsync(context, expected1, expected2);

				var actual1 = await FindAsync(context, 1);
				Assert.Equal(expected1.Id, actual1.Id);
				Assert.Equal(expected1.Name, actual1.Name);
				Assert.Equal(expected1.Category, actual1.Category);

				var actual2 = await FindAsync(context, 2);
				Assert.Equal(expected2.Id, actual2.Id);
				Assert.Equal(expected2.Name, actual2.Name);
				Assert.Equal(expected2.Category, actual2.Category);
			} catch (Exception) {
				AssertHelper.Fail();
			} finally {
				await DropTableAsync(context);
			}
		}
	}
}
