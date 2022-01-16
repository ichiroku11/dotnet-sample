using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

// 参考
// https://docs.microsoft.com/ja-jp/learn/modules/build-web-api-minimal-api/
// https://docs.microsoft.com/ja-jp/learn/modules/build-web-api-minimal-database/

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MonsterDbContext>(options => {
	options.UseInMemoryDatabase("monster");
});
builder.Services.AddScoped<MonsterStore>();

var app = builder.Build();

// 取得
app.MapGet("/monsters", (MonsterStore store) => store.GetMonsters());

// 取得
app.MapGet("/monsters/{id}", (MonsterStore store, int id) => {
	var monster = store.GetMonster(id);
	if (monster is null) {
		return Results.NotFound();
	}

	return Results.Ok(monster);
});

// 作成
app.MapPost("/monsters", (MonsterStore store, Monster monster) => {
	return store.TryAddMonster(monster)
		? Results.NoContent()
		: Results.BadRequest();
});

// 更新
app.MapPut("/monsters", (MonsterStore store, Monster monster) => {
	return store.TryUpdateMonster(monster)
		? Results.NoContent()
		: Results.BadRequest();
});

// 削除
app.MapDelete("/monsters/{id}", (MonsterStore store, int id) => {
	return store.TryDeleteMonster(id)
		? Results.NoContent()
		: Results.BadRequest();
});

app.MapGet("/", () => "Hello World!");

app.Run();

internal record Monster(int Id, string Name);

internal class MonsterDbContext : DbContext {
	public MonsterDbContext(DbContextOptions options) : base(options) {
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);
	}
}

internal class MonsterStore {
	private static readonly ConcurrentDictionary<int, Monster> _monsters
		= new(new[] {
			new Monster(1, "スライム"),
			new Monster(2, "ドラキー"),
		}.ToDictionary(monster => monster.Id));

	public IList<Monster> GetMonsters() => _monsters.Values.OrderBy(monster => monster.Id).ToList();

	public Monster? GetMonster(int id) => _monsters.TryGetValue(id, out var monster) ? monster : null;

	public bool TryAddMonster(Monster monster) => _monsters.TryAdd(monster.Id, monster);

	public bool TryUpdateMonster(Monster monsterToUpdate) {
		if (!_monsters.TryGetValue(monsterToUpdate.Id, out var monster)) {
			return false;
		}

		return _monsters.TryUpdate(monsterToUpdate.Id, monsterToUpdate, monster);
	}

	public bool TryDeleteMonster(int id) => _monsters.TryRemove(id, out var _);
}
