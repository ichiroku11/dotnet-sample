using RazorPageWebApp.Models.Monsters;
using SampleLib.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace RazorPageWebApp.Pages.Monsters;

public class EditFormModel {
	[Range(1, 999)]
	public int Id { get; set; }

	[AllowedEnum<MonsterCategory>(excludes: MonsterCategory.None)]
	public string Category { get; set; } = "";

	[Length(2, 10)]
	public string Name { get; set; } = "";

	public Monster ToMonster() => Monster.Create(Id, Category, Name);
}
