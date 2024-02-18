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

		var result = monsters
			.OrderBy(monster => monster.Id)
			.ToList();
		return Task.FromResult<IList<Monster>>(result);
	}

	public Task<Monster?> GetByIdAsync(int id) {
		var result = _monsters.TryGetValue(id, out var monster)
			? monster
			: null;
		return Task.FromResult(result);
	}

	public Task<bool> AddAsync(Monster monster) {
		var result = _monsters.TryAdd(monster.Id, monster);

		return Task.FromResult(result);
	}
}
