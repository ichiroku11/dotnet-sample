using System.Collections.Concurrent;

// 参考
// https://docs.microsoft.com/ja-jp/learn/modules/build-web-api-minimal-api/
// https://docs.microsoft.com/ja-jp/learn/modules/build-web-api-minimal-database/

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/monsters", () => {
	var store = new MonsterStore();
	return store.GetMonsters();
});


app.Run();

internal class Monster {
	public int Id { get; init; }
	public string Name { get; init; } = "";
}

internal class MonsterStore {
	private static readonly ConcurrentDictionary<int, Monster> _monsters
		= new(new[] {
			new Monster {
				Id = 1,
				Name = "スライム", 
			},
			new Monster {
				Id = 2,
				Name = "ドラキー",
			},
		}.ToDictionary(monster => monster.Id));

	public IList<Monster> GetMonsters() => _monsters.Values.OrderBy(monster => monster.Id).ToList();
}
