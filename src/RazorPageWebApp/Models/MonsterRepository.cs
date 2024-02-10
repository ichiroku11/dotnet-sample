using System.Collections.Concurrent;

namespace RazorPageWebApp.Models;

public class MonsterRepository {
	private static readonly ConcurrentDictionary<int, Monster> _monsters
		= new(new[] {
			new Monster(1, "スライム"),
			new Monster(2, "ドラキー"),
		}.Select(monster => KeyValuePair.Create(monster.Id, monster)));

	public IList<Monster> QueryAll() => _monsters.Values.OrderBy(monster => monster.Id).ToList();
}
