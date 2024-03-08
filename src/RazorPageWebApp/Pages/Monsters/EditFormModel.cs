using RazorPageWebApp.Models.Monsters;
using SampleLib.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace RazorPageWebApp.Pages.Monsters;

public class EditFormModel {
	[Range(1, 999)]
	public int Id { get; set; }

	[AllowedEnum<MonsterCategory>]
	public string Category { get; set; } = "";

	[Length(2, 10)]
	public string Name { get; set; } = "";

	// todo: MonsterCategory
	public Monster ToMonster() => new(Id, MonsterCategory.None, Name);

}
