using Microsoft.EntityFrameworkCore;

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
app.MapGet("/monsters", async (MonsterStore store) => await store.GetAsync());

// 取得
app.MapGet("/monsters/{id}", async (MonsterStore store, int id) => {
	var monster = await store.GetAsync(id);
	if (monster is null) {
		return Results.NotFound();
	}

	return Results.Ok(monster);
});

// 作成
app.MapPost("/monsters", async (MonsterStore store, Monster monster) => {
	if ((await store.GetAsync(monster.Id)) is not null) {
		return Results.BadRequest();
	}

	await store.AddAsync(monster);

	return Results.NoContent();
});

// 更新
app.MapPut("/monsters", async (MonsterStore store, Monster monster) => {
	if ((await store.GetAsync(monster.Id)) is null) {
		return Results.BadRequest();
	}

	await store.UpdateAsync(monster);

	return Results.NoContent();
});

// 削除
app.MapDelete("/monsters/{id}", async (MonsterStore store, int id) => {
	var monster = await store.GetAsync(id);
	if (monster is null) {
		return Results.BadRequest();
	}

	await store.DeleteAsync(monster);

	return Results.NoContent();
});

app.MapGet("/", () => "Hello World!");

app.Run();

internal record Monster(int Id, string Name);

internal class MonsterDbContext : DbContext {
	public MonsterDbContext(DbContextOptions options) : base(options) {
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Monster>()
			// 初期データ
			// 設定されない？
			.HasData(new Monster(1, "スライム"), new Monster(2, "ドラキー"));
	}
}

internal class MonsterStore {
	private readonly MonsterDbContext _context;

	public MonsterStore(MonsterDbContext context) {
		_context = context;
	}

	public async Task<IList<Monster>> GetAsync()
		=> await _context.Set<Monster>().OrderBy(monster => monster.Id).ToListAsync();

	public async Task<Monster?> GetAsync(int id)
		=> await _context.Set<Monster>().FindAsync(id);

	public async Task AddAsync(Monster monster) {
		_context.Set<Monster>().Add(monster);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateAsync(Monster monster) {
		_context.Set<Monster>().Update(monster);
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(Monster monster) {
		_context.Set<Monster>().Remove(monster);
		await _context.SaveChangesAsync();
	}
}
