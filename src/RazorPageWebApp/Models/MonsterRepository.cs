using System.Collections.Concurrent;

namespace RazorPageWebApp.Models;

public class MonsterRepository {
	private static readonly ConcurrentDictionary<int, Monster> _monsters
		= new(new[] {
			new Monster(1, "スライム"),
			new Monster(2, "ドラキー"),
		}.Select(monster => KeyValuePair.Create(monster.Id, monster)));

	public Task<IList<Monster>> QueryAsync(MonsterListQueryOption option) {
		IEnumerable<Monster> monsters = _monsters.Values;

		if (!string.IsNullOrWhiteSpace(option.Query)) {
			monsters = monsters.Where(monster => monster.Name.Contains(option.Query));
		}

		return Task.FromResult<IList<Monster>>(
			monsters
				.OrderBy(monster => monster.Id)
				.ToList());
	}
}
