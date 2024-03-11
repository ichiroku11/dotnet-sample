using SampleLib;

namespace RazorPageWebApp.Models.Monsters;

public class Monster(int id, MonsterCategory category, string name) {
	public static Monster Create(int id, string category, string name)
		=> new(id, Enum.Parse<MonsterCategory>(category, true), name);

	public int Id { get; private set; } = id;

	public MonsterCategory Category { get; private set; } = category;

	public string CategoryName => Category.DisplayName() ?? "";

	public string Name { get; private set; } = name;

	public void CopyFrom(Monster from) {
		Id = from.Id;
		Category = from.Category;
		Name = from.Name;
	}
}
