namespace RazorPageWebApp.Models.Monsters;

public class Monster(int id, MonsterCategory category, string name) {
	public int Id { get; private set; } = id;

	public MonsterCategory Category { get; private set; } = category;

	public string Name { get; private set; } = name;

	public void CopyFrom(Monster from) {
		Id = from.Id;
		Category = from.Category;
		Name = from.Name;
	}
}
