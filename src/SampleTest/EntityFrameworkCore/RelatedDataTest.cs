using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.EntityFrameworkCore {
	// 参考
	// https://docs.microsoft.com/ja-jp/ef/core/querying/related-data
	// https://docs.microsoft.com/ja-jp/ef/core/saving/related-data
	[Collection("dbo.Monster")]
	public class RelatedDataTest : IDisposable {
		private static class EqualityComparerFactory<TElement> {
			public static EqualityComparer<TElement> Create<TKey>(Func<TElement, TKey> keySelector) {
				return new EqualityComparer<TElement, TKey>(keySelector);
			}
		}

		private class MonsterCategory {
			public int Id { get; set; }
			public string Name { get; set; }
		}

		private class Monster {
			public int Id { get; set; }
			public int CategoryId { get; set; }
			public string Name { get; set; }

			// Navigation
			public MonsterCategory Category { get; set; }
			public List<MonsterItem> Items { get; set; }
		}
		private class Item {
			public int Id { get; set; }
			public string Name { get; set; }
		}

		private class MonsterItem {
			public int MonsterId { get; set; }
			public int ItemId { get; set; }

			// Navigation
			public Monster Monster { get; set; }
			public Item Item { get; set; }
		}

		private static readonly EqualityComparer<MonsterCategory> _monsterCategoryComparer
			= EqualityComparerFactory<MonsterCategory>.Create(category => new { category.Id, category.Name });

		private static readonly EqualityComparer<Monster> _monsterComparer
			= EqualityComparerFactory<Monster>.Create(monster => new { monster.Id, monster.CategoryId, monster.Name });

		private static readonly EqualityComparer<Item> _itemComparer
			= EqualityComparerFactory<Item>.Create(item => new { item.Id, item.Name });

		private static readonly EqualityComparer<MonsterItem> _monsterItemComparer
			= EqualityComparerFactory<MonsterItem>.Create(item => new { item.MonsterId, item.ItemId });

		private static readonly IReadOnlyDictionary<int, MonsterCategory> _monsterCategories
			= new Dictionary<int, MonsterCategory>() {
				{ 1, new MonsterCategory { Id = 1, Name = "Slime", } },
				{ 2, new MonsterCategory { Id = 2, Name = "Animal", } },
				{ 3, new MonsterCategory { Id = 3, Name = "Fly", } }
			};

		private static readonly IReadOnlyCollection<Monster> _monsters
			= new[] {
				new Monster { Id = 1, CategoryId = 1, Name = "スライム", },
				new Monster { Id = 2, CategoryId = 2, Name = "ドラキー", },
			};

		private static readonly IReadOnlyDictionary<int, Item> _items
			= new Dictionary<int, Item> {
				{ 1, new Item { Id = 1, Name = "やくそう", } },
				{ 2, new Item { Id = 2, Name = "スライムゼリー", } },
				{ 3, new Item { Id = 3, Name = "キメラのつばさ", } },
			};

		private static readonly IReadOnlyCollection<MonsterItem> _monsterItems
			= new[] {
				// スライム => やくそう、スライムゼリー
				new MonsterItem { MonsterId = 1, ItemId = 1, },
				new MonsterItem { MonsterId = 1, ItemId = 2, },
				// ドラキー => やくそう、キメラのつばさ
				new MonsterItem { MonsterId = 2, ItemId = 1, },
				new MonsterItem { MonsterId = 2, ItemId = 3, },
			};

		private class MonsterDbContext : AppDbContext {
			public DbSet<MonsterCategory> MonsterCategories { get; set; }
			public DbSet<Monster> Monsters { get; set; }
			public DbSet<Item> Items { get; set; }
			public DbSet<MonsterItem> MonsterItems { get; set; }

			protected override void OnModelCreating(ModelBuilder modelBuilder) {
				modelBuilder.Entity<MonsterCategory>().ToTable(nameof(MonsterCategory));
				modelBuilder.Entity<Monster>().ToTable(nameof(Monster));
				modelBuilder.Entity<Item>().ToTable(nameof(Item));
				modelBuilder.Entity<MonsterItem>().ToTable(nameof(MonsterItem))
					// 複合主キー
					.HasKey(monsterItem => new { monsterItem.MonsterId, monsterItem.ItemId });
			}
		}

		private MonsterDbContext _context;

		public RelatedDataTest() {
			_context = new MonsterDbContext();

			DropTable();
			CreateTable();
		}

		public void Dispose() {
			DropTable();

			if (_context != null) {
				_context.Dispose();
				_context = null;
			}
		}

		private int CreateTable() {
			var sql = @"
create table dbo.MonsterCategory(
	Id int not null,
	Name nvarchar(10) not null,
	constraint PK_MonsterCategory primary key(Id)
);

create table dbo.Monster(
	Id int not null,
	CategoryId int not null,
	Name nvarchar(10) not null,
	constraint PK_Monster primary key(Id),
	constraint FK_Monster_MonsterCategory
		foreign key(CategoryId) references dbo.MonsterCategory(Id)
);

create table dbo.Item(
	Id int not null,
	Name nvarchar(10) not null,
	constraint PK_Item primary key(Id),
);

create table dbo.MonsterItem(
	MonsterId int not null,
	ItemId int not null,
	constraint PK_MonsterItem primary key(MonsterId, ItemId),
	constraint FK_MonsterItem_Monster
		foreign key(MonsterId) references dbo.Monster(Id),
	constraint FK_MonsterItem_Item
		foreign key(ItemId) references dbo.Item(Id)
);";

			return _context.Database.ExecuteSqlRaw(sql);
		}

		private int DropTable() {
			var sql = @"
drop table if exists dbo.MonsterItem;
drop table if exists dbo.Item;
drop table if exists dbo.Monster;
drop table if exists dbo.MonsterCategory;";

			return _context.Database.ExecuteSqlRaw(sql);
		}

		private Task<int> InitAsync() {
			var sql = @"
delete from dbo.MonsterItem;
delete from dbo.Item;
delete from dbo.Monster;
delete from dbo.MonsterCategory;";

			return _context.Database.ExecuteSqlRawAsync(sql);
		}

		[Fact]
		public async Task モンスターカテゴリを追加して取得できる() {
			// Arrange
			await InitAsync();

			var expected = _monsterCategories.Values.OrderBy(category => category.Id);

			// Act
			// Assert

			// カテゴリを追加（追加した件数が正しい）
			_context.MonsterCategories.AddRange(expected);
			var rows = await _context.SaveChangesAsync();
			Assert.Equal(expected.Count(), rows);

			// 追加したカテゴリを取得できる
			var actual = await _context.MonsterCategories
				.OrderBy(category => category.Id)
				.ToListAsync();
			Assert.Equal(expected, actual, _monsterCategoryComparer);
		}

		// AddRangeでMonsterとMonsterCategoryを同時にinsert
		// Monster一覧を取得（Includeを使わずに）
		// MonsterCategory一覧を取得
		[Fact]
		public async Task モンスターを追加する際にカテゴリも追加できる() {
			// Arrange
			await InitAsync();

			var expectedMonsters = _monsters
				.Select(monster => new Monster {
					Id = monster.Id,
					CategoryId = monster.CategoryId,
					Name = monster.Name,
					// ナビゲーションプロパティにカテゴリを設定
					Category = _monsterCategories[monster.CategoryId]
				})
				.OrderBy(monster => monster.Id);
			var expectedCategories = expectedMonsters
				.Select(monster => monster.Category)
				.Distinct(_monsterCategoryComparer);

			// Act
			// Assert
			// モンスターを追加
			// ナビゲーションプロパティに設定されているカテゴリも追加される
			_context.Monsters.AddRange(expectedMonsters);
			var rows = await _context.SaveChangesAsync();

			// カテゴリも追加された数になる
			Assert.Equal(expectedMonsters.Count() + expectedCategories.Count(), rows);

			// 追加したモンスターを取得できる
			var actualMonsters = await _context.Monsters
				.OrderBy(category => category.Id)
				.ToListAsync();
			Assert.Equal(expectedMonsters, actualMonsters, _monsterComparer);
			// Categoryプロパティはすべてnull
			Assert.All(actualMonsters, monster => Assert.Null(monster.Category));

			// 追加されたカテゴリを取得できる
			var actualCategories = await _context.MonsterCategories
				.OrderBy(category => category.Id)
				.ToListAsync();
			Assert.Equal(expectedCategories, actualCategories, _monsterCategoryComparer);
		}

		// Includeで1対1の関連データを読み込む
		[Fact]
		public async Task Include_OneOne() {
			// Arrange
			await InitAsync();

			// カテゴリを追加
			_context.MonsterCategories.AddRange(_monsterCategories.Values);
			// モンスターを追加
			var expected = _monsters.OrderBy(monster => monster.Id);
			_context.Monsters.AddRange(expected);

			var rows = await _context.SaveChangesAsync();
			Assert.Equal(expected.Count() + _monsterCategories.Count(), rows);

			// Act
			// Assert
			// Includeを使わずにモンスター一覧を取得
			var actual = await _context.Monsters
				.OrderBy(category => category.Id)
				.ToListAsync();
			Assert.Equal(expected, actual, _monsterComparer);
			// Categoryプロパティはすべてnull
			Assert.All(actual, monster => Assert.Null(monster.Category));

			// Includeを使ってモンスター一覧とカテゴリをあわせて取得
			actual = await _context.Monsters
				.Include(monster => monster.Category)
				.OrderBy(monster => monster.Id)
				.ToListAsync();
			Assert.Equal(expected, actual, _monsterComparer);
			// Categoryプロパティが設定されている
			Assert.All(
				actual,
				monster => Assert.Equal(
					_monsterCategories[monster.CategoryId],
					monster.Category,
					_monsterCategoryComparer));
		}

		// Includeで1対多の関連データを読み込む
		[Fact]
		public async Task Include_OneMany() {
			// Arrange
			await InitAsync();

			// カテゴリを追加
			_context.MonsterCategories.AddRange(_monsterCategories.Values);
			// アイテムを追加
			_context.Items.AddRange(_items.Values);
			// モンスターとモンスターアイテムを追加
			var expected = _monsters
				.Select(monster => new Monster {
					Id = monster.Id,
					CategoryId = monster.CategoryId,
					Name = monster.Name,
					// ナビゲーションプロパティにMonsterItemを設定
					Items = _monsterItems.Where(item => item.MonsterId == monster.Id).ToList(),
				})
				.OrderBy(monster => monster.Id);
			_context.Monsters.AddRange(expected);

			var rows = await _context.SaveChangesAsync();
			Assert.Equal(_monsterCategories.Count() + _items.Count() + expected.Count() + _monsterItems.Count(), rows);

			// Act
			// Assert
			// Includeを使わずにモンスター一覧を取得
			var actual = await _context.Monsters
				.OrderBy(monster => monster.Id)
				.ToListAsync();
			Assert.Equal(expected, actual, _monsterComparer);
			// Itemsプロパティはすべてnull
			Assert.All(actual, monster => Assert.Null(monster.Items));

			// Includeを使ってモンスター一覧とアイテム一覧をあわせて取得
			actual = await _context.Monsters
				.Include(monster => monster.Items)
				.OrderBy(monster => monster.Id)
				.ToListAsync();
			Assert.Equal(expected, actual, _monsterComparer);
			// Itemsプロパティが設定されている
			Assert.All(
				actual,
				actual => Assert.Equal(
					expected.FirstOrDefault(monster => monster.Id == actual.Id).Items,
					actual.Items,
					_monsterItemComparer));
		}

		// IncludeとThenIncludeで多対多の関連データを読み込む
		[Fact]
		public async Task Include_ManyMany() {
			// Arrange
			await InitAsync();

			// カテゴリを追加
			_context.MonsterCategories.AddRange(_monsterCategories.Values);
			// アイテムを追加
			_context.Items.AddRange(_items.Values);
			// モンスターとモンスターアイテムを追加
			var expected = _monsters
				.Select(monster => new Monster {
					Id = monster.Id,
					CategoryId = monster.CategoryId,
					Name = monster.Name,
					// ナビゲーションプロパティにMonsterItemを設定
					Items = _monsterItems.Where(item => item.MonsterId == monster.Id).ToList(),
				})
				.OrderBy(monster => monster.Id);
			_context.Monsters.AddRange(expected);

			var rows = await _context.SaveChangesAsync();
			Assert.Equal(_monsterCategories.Count() + _items.Count() + expected.Count() + _monsterItems.Count(), rows);

			// Act
			// Assert
			// IncludeとThenIncludeを使ってモンスター一覧とアイテム一覧、アイテムをあわせて取得
			var actual = await _context.Monsters
				.Include(monster => monster.Items)
					.ThenInclude(monsterItem => monsterItem.Item)
				.OrderBy(monster => monster.Id)
				.ToListAsync();
			Assert.Equal(expected, actual, _monsterComparer);
			// Itemsプロパティが設定されている
			Assert.All(
				actual,
				actual => Assert.Equal(
					expected.FirstOrDefault(monster => monster.Id == actual.Id).Items,
					actual.Items,
					_monsterItemComparer));
			// ItemプロパティのItemが正しい
			Assert.All(
				actual.SelectMany(monster => monster.Items),
				actual => Assert.Equal(
					_items[actual.ItemId],
					actual.Item,
					_itemComparer));
		}
	}
}
