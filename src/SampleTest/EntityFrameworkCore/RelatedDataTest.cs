using Microsoft.EntityFrameworkCore;
using SampleLib;

namespace SampleTest.EntityFrameworkCore;

// 参考
// https://docs.microsoft.com/ja-jp/ef/core/querying/related-data
// https://docs.microsoft.com/ja-jp/ef/core/saving/related-data
[Collection(CollectionNames.EfCoreMonster)]
public class RelatedDataTest : IDisposable {
	private static class EqualityComparerFactory<TElement> where TElement : notnull {
		public static EqualityComparer<TElement> Create<TKey>(Func<TElement, TKey> keySelector) where TKey : notnull {
			return new EqualityComparer<TElement, TKey>(keySelector);
		}
	}

	private class MonsterCategory {
		public int Id { get; init; }
		public string Name { get; init; } = "";
	}

	private class Monster {
		public int Id { get; init; }
		public int CategoryId { get; init; }
		public string Name { get; init; } = "";

		// Navigation
		public MonsterCategory? Category { get; init; }
		public List<MonsterItem> Items { get; init; } = [];
	}

	private class Item {
		public int Id { get; init; }
		public string Name { get; init; } = "";
	}

	private class MonsterItem {
		public int MonsterId { get; init; }
		public int ItemId { get; init; }

		// Navigation
		public Monster? Monster { get; init; }
		public Item? Item { get; init; }
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
		= [
			new Monster { Id = 1, CategoryId = 1, Name = "スライム", },
			new Monster { Id = 2, CategoryId = 2, Name = "ドラキー", },
		];

	private static readonly IReadOnlyDictionary<int, Item> _items
		= new Dictionary<int, Item> {
			{ 1, new Item { Id = 1, Name = "やくそう", } },
			{ 2, new Item { Id = 2, Name = "スライムゼリー", } },
			{ 3, new Item { Id = 3, Name = "キメラのつばさ", } },
		};

	private static readonly IReadOnlyCollection<MonsterItem> _monsterItems
		= [
			// スライム => やくそう、スライムゼリー
			new MonsterItem { MonsterId = 1, ItemId = 1, },
			new MonsterItem { MonsterId = 1, ItemId = 2, },
			// ドラキー => やくそう、キメラのつばさ
			new MonsterItem { MonsterId = 2, ItemId = 1, },
			new MonsterItem { MonsterId = 2, ItemId = 3, },
		];

	private class MonsterDbContext : SqlServerDbContext {
		public DbSet<MonsterCategory> MonsterCategories => Set<MonsterCategory>();
		public DbSet<Monster> Monsters => Set<Monster>();
		public DbSet<Item> Items => Set<Item>();
		public DbSet<MonsterItem> MonsterItems => Set<MonsterItem>();

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<MonsterCategory>().ToTable(nameof(MonsterCategory));
			modelBuilder.Entity<Monster>().ToTable(nameof(Monster));
			modelBuilder.Entity<Item>().ToTable(nameof(Item));
			modelBuilder.Entity<MonsterItem>().ToTable(nameof(MonsterItem))
				// 複合主キー
				.HasKey(monsterItem => new { monsterItem.MonsterId, monsterItem.ItemId });
		}
	}

	private readonly MonsterDbContext _context = new();

	public RelatedDataTest() {
		DropTable();
		CreateTable();
	}

	public void Dispose() {
		DropTable();

		_context.Dispose();

		GC.SuppressFinalize(this);
	}

	private int CreateTable() {
		FormattableString sql = $@"
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

		return _context.Database.ExecuteSql(sql);
	}

	private int DropTable() {
		FormattableString sql = $@"
drop table if exists dbo.MonsterItem;
drop table if exists dbo.Item;
drop table if exists dbo.Monster;
drop table if exists dbo.MonsterCategory;";

		return _context.Database.ExecuteSql(sql);
	}

	private Task<int> InitAsync() {
		FormattableString sql = $@"
delete from dbo.MonsterItem;
delete from dbo.Item;
delete from dbo.Monster;
delete from dbo.MonsterCategory;";

		return _context.Database.ExecuteSqlAsync(sql);
	}

	private static IEnumerable<Monster> GetMonsters()
		=> _monsters
			.Select(monster => new Monster {
				Id = monster.Id,
				CategoryId = monster.CategoryId,
				Name = monster.Name,
				// ナビゲーションプロパティにMonsterItemを設定
				Items = _monsterItems.Where(item => item.MonsterId == monster.Id).ToList(),
			})
			.OrderBy(monster => monster.Id);

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
			.Select(monster => monster.Category!)
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
		Assert.Equal(expected.Count() + _monsterCategories.Count, rows);

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
				monster.Category!,
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
		var expected = GetMonsters();
		_context.Monsters.AddRange(expected);

		var rows = await _context.SaveChangesAsync();
		Assert.Equal(_monsterCategories.Count + _items.Count + expected.Count() + _monsterItems.Count, rows);

		// Act
		// Assert
		// Includeを使わずにモンスター一覧を取得
		var actual = await _context.Monsters
			.OrderBy(monster => monster.Id)
			.ToListAsync();
		Assert.Equal(expected, actual, _monsterComparer);
		// Itemsプロパティはすべて空
		Assert.All(actual, monster => Assert.Empty(monster.Items));

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
				expected.First(monster => monster.Id == actual.Id).Items,
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
		var expected = GetMonsters();
		_context.Monsters.AddRange(expected);

		var rows = await _context.SaveChangesAsync();
		Assert.Equal(_monsterCategories.Count + _items.Count + expected.Count() + _monsterItems.Count, rows);

		// Act
		// IncludeとThenIncludeを使ってモンスター一覧とアイテム一覧、アイテムをあわせて取得
		var actual = await _context.Monsters
			.Include(monster => monster.Items!)
				.ThenInclude(monsterItem => monsterItem.Item)
			.OrderBy(monster => monster.Id)
			.ToListAsync();

		// Assert
		Assert.Equal(expected, actual, _monsterComparer);
		// Itemsプロパティが設定されている
		Assert.All(
			actual,
			actual => Assert.Equal(
				expected.First(monster => monster.Id == actual.Id).Items,
				actual.Items,
				_monsterItemComparer));
		// ItemプロパティのItemが正しい
		Assert.All(
			actual.SelectMany(monster => monster.Items!),
			actual => Assert.Equal(
				_items[actual.ItemId],
				actual.Item!,
				_itemComparer));
	}

	// Includeでフィルターする
	[Fact]
	public async Task Include_Filtered() {
		// Arrange
		await InitAsync();

		_context.MonsterCategories.AddRange(_monsterCategories.Values);
		_context.Items.AddRange(_items.Values);
		var monsters = GetMonsters();
		_context.Monsters.AddRange(monsters);

		var rows = await _context.SaveChangesAsync();
		Assert.Equal(_monsterCategories.Count + _items.Count + monsters.Count() + _monsterItems.Count, rows);

		// Act
		// IncludeするItemを「スライムゼリー」だけフィルターする
		var actual = await _context.Monsters
			.Include(monster => monster.Items!.Where(item => item.ItemId == 2))
			.OrderBy(monster => monster.Id)
			.ToListAsync();

		// Assert
		// 発行されるSQLはleft joinのようでMonster自体はフィルターされない
		Assert.Equal(monsters, actual, _monsterComparer);

		// Itemsプロパティはフィルターされる
		Assert.Collection(actual,
			monster => {
				// スライム
				Assert.Equal(1, monster.Id);
				Assert.Single(monster.Items);
				var monsterItem = monster.Items!.First();
				Assert.Equal(2, monsterItem.ItemId);
			},
			monster => {
				// ドラキー
				Assert.Equal(2, monster.Id);
				Assert.Empty(monster.Items);
			});
	}

	// Includeでフィルターし、フィルターされた子要素だけを持つ要素に絞り込む
	[Fact]
	public async Task Include_Filtered2() {
		// Arrange
		await InitAsync();

		_context.MonsterCategories.AddRange(_monsterCategories.Values);
		_context.Items.AddRange(_items.Values);
		var monsters = GetMonsters();
		_context.Monsters.AddRange(GetMonsters());

		var rows = await _context.SaveChangesAsync();
		Assert.Equal(_monsterCategories.Count + _items.Count + monsters.Count() + _monsterItems.Count, rows);

		// Act
		// IncludeするItemを「スライムゼリー」だけにフィルターし、
		// さらに「スライムゼリー」を持つMonsterだけに絞り込む
		var actual = await _context.Monsters
			.Include(monster => monster.Items!.Where(item => item.ItemId == 2))
			.Where(monster => monster.Items!.Any(item => item.ItemId == 2))
			.OrderBy(monster => monster.Id)
			.ToListAsync();

		// Assert
		// スライムだけを取得できる
		Assert.Single(actual);

		var monster = actual.First();
		Assert.Equal(1, monster.Id);
		Assert.Single(monster.Items);

		var monsterItem = monster.Items!.First();
		Assert.Equal(2, monsterItem.ItemId);
	}
}
