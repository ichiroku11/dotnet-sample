using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleTest.EntityFrameworkCore;

// 主キーや外部キーをenumのプロパティにマッピングするサンプル
[Collection(CollectionNames.EfCoreMonster)]
public class EnumPrimaryKeyForeignKeyTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	private enum MonsterCategoryType {
		None = 0,
		Slime,
		Animal,
		Fly,
	}

	private class MonsterCategory {
		// 主キー
		[Key, Column("Id")]
		public MonsterCategoryType Type { get; set; }
		public string Name { get; set; } = "";
	}

	private class Monster {
		public int Id { get; set; }
		// 外部キー
		[Column("CategoryId")]
		public MonsterCategoryType CategoryType { get; set; }
		public string Name { get; set; } = "";
		public MonsterCategory? Category { get; set; }
	}

	private class MonsterDbContext : SqlServerDbContext {
		public DbSet<MonsterCategory> MonsterCategories => Set<MonsterCategory>();
		public DbSet<Monster> Monsters => Set<Monster>();

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<MonsterCategory>().ToTable(nameof(MonsterCategory));
			modelBuilder.Entity<Monster>().ToTable(nameof(Monster));
		}
	}

	private static async Task CreateTableAsync(MonsterDbContext context) {
		var sql = @"
create table dbo.MonsterCategory(
	Id int,
	Name nvarchar(20) not null,
	constraint PK_MonsterCategory primary key(Id)
);
create table dbo.Monster(
	Id int,
	Name nvarchar(20) not null,
	CategoryId int,
	constraint PK_Monster primary key(Id),
	constraint FK_Monster_MonsterCategory foreign key(CategoryId)
		references dbo.MonsterCategory(Id)
);";
		await context.Database.ExecuteSqlRawAsync(sql);
	}

	private static async Task DropTableAsync(MonsterDbContext context) {
		var sql = @"
drop table if exists dbo.Monster;
drop table if exists dbo.MonsterCategory;";
		await context.Database.ExecuteSqlRawAsync(sql);
	}

	private static async Task InitTableAsync(MonsterDbContext context) {
		var monsterCategories = new[] {
			new MonsterCategory {
				Type = MonsterCategoryType.Slime,
				Name = "スライム系",
			},
			new MonsterCategory {
				Type = MonsterCategoryType.Animal,
				Name = "けもの系",
			},
			new MonsterCategory {
				Type = MonsterCategoryType.Fly,
				Name = "鳥系",
			},
		};

		var monsters = new[] {
			new Monster {
				Id = 1,
				Name = "スライム",
				CategoryType = MonsterCategoryType.Slime,
			},
			new Monster {
				Id = 2,
				Name = "ドラキー",
				CategoryType = MonsterCategoryType.Fly,
			},
		};

		context.MonsterCategories.AddRange(monsterCategories);
		context.Monsters.AddRange(monsters);

		await context.SaveChangesAsync();
	}

	[Fact]
	public async Task 主キーや外部キーのID列をEnumにバインドできる() {
		using var context = new MonsterDbContext();

		try {
			await DropTableAsync(context);
			await CreateTableAsync(context);
			await InitTableAsync(context);

			var monsters = await context.Monsters
				.Where(monster => monster.CategoryType == MonsterCategoryType.Slime)
				.ToListAsync();
			Assert.Single(monsters);

			var monster = monsters.First();
			Assert.Equal(1, monster.Id);
			Assert.Equal("スライム", monster.Name);
			Assert.Equal(MonsterCategoryType.Slime, monster.CategoryType);
			Assert.Null(monster.Category);
		} catch (Exception exception) {
			_output.WriteLine(exception.ToString());
			Assert.Fail();
		} finally {
			await DropTableAsync(context);
		}
	}

	[Fact]
	public async Task 主キーや外部キーのID列をEnumにバインドできてインクルードもできる() {
		using var context = new MonsterDbContext();

		try {
			await DropTableAsync(context);
			await CreateTableAsync(context);
			await InitTableAsync(context);

			var monster = (await context.Monsters
				// カテゴリをインクルード
				.Include(monster => monster.Category)
				.FirstOrDefaultAsync(monster => monster.CategoryType == MonsterCategoryType.Fly))!;

			Assert.NotNull(monster);
			Assert.Equal(2, monster.Id);
			Assert.Equal("ドラキー", monster.Name);
			Assert.Equal(MonsterCategoryType.Fly, monster.CategoryType);
			Assert.Equal(MonsterCategoryType.Fly, monster.Category?.Type);
			Assert.Equal("鳥系", monster.Category?.Name);
		} catch (Exception exception) {
			_output.WriteLine(exception.ToString());
			Assert.Fail();
		} finally {
			await DropTableAsync(context);
		}
	}
}
