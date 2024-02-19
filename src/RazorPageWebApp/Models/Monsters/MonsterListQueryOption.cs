namespace RazorPageWebApp.Models.Monsters;

public class MonsterListQueryOption(string query) {
	public string Query { get; } = query;
}
