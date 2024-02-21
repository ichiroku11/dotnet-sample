namespace RazorPageWebApp.Models.Monsters;

public class Monster(int id, string name) {
	public int Id { get; private set; } = id;

	public string Name { get; private set; } = name;

	public void CopyFrom(Monster from) {
		Id = from.Id;
		Name = from.Name;
	}
}
